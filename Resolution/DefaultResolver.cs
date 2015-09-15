using System.Text;
using ItzWarty;
using ItzWarty.Collections;
using ItzWarty.Comparers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using Dargon.IO.Utilities;

namespace Dargon.IO.Resolution
{
   public class DefaultResolver : Resolver {
      private readonly object initializationSynchronization = new object();
      private readonly MultiValueDictionary<string, ReadableDargonNode> nodesByNameInsensitive = new MultiValueDictionary<string, ReadableDargonNode>(new CaseInsensitiveStringEqualityComparer());
      private readonly Logger logger;
      private readonly ReadableDargonNode root;
      private bool initialized = false;

      public DefaultResolver(Logger logger, ReadableDargonNode root) {
         this.logger = logger;
         this.root = root;
      }

      private void Initialize() {
         if (initialized) return;
         lock (initializationSynchronization) {
            if (initialized) return;

            initialized = true;

            foreach (var leaf in root.GetLeaves()) {
               nodesByNameInsensitive.Add(leaf.Name, leaf);
            }
         }
      }

      public IReadOnlyList<ReadableDargonNode> Resolve(string inputPath, string hintPath)
      {
         if (hintPath != null) {
            logger.Info("ATTEMPT RESOLVE HINT " + inputPath);
            var result = root.GetRelativeOrNull<ReadableDargonNode>(hintPath);
            if (result != null) {
               return new[] { result };
            }
         }

         Initialize();

         //----------------------------------------------------------------------------------------
         // Split the file path into individual file system object tokens.
         // ex: FileTree, Characters, Annie, Annie.dds
         // ex: FileTree, Characters, Master Yi, MasterYiLoadScreen.dds
         //----------------------------------------------------------------------------------------
         string[] inputPathBreadCrumbs = PathUtilities.GetPathBreadCrumbs(inputPath);

         var initialCandidateNodes = nodesByNameInsensitive.GetValueOrDefault(inputPathBreadCrumbs.Last());

         if (initialCandidateNodes == null) {
            return new List<ReadableDargonNode>();
         }

         //----------------------------------------------------------------------------------------
         // Step 1: we've just enumerated all resources (as seen above).
         // Step 2: We start with an initial search, ex Annie.dds
         // Step 3: We look at our list of enumerated resources, and filter out resources that do
         //         not end with Annie.dds.  If we end up with zero results, we fall back to our
         //         previous nodes.
         // Step 4: We do the above at least twice, before we move on to more expensive operations.
         //         Our expensive options involve looking at our resource nodeImpl's raf-id-less paths.
         //         If the no-raf-id paths match, then we just have a duplicated file, so we return
         //         that match.
         //----------------------------------------------------------------------------------------
         var currentCandidateNodes = new List<ReadableDargonNode>(32);
         var nextCandidateNodes = new List<ReadableDargonNode>(32);

         bool doFinalize = false;
         for (int currentBreadcrumbIndex = inputPathBreadCrumbs.Length - 1, iteration = 0;
            !doFinalize && (currentBreadcrumbIndex >= 0 && (currentCandidateNodes.Any() || iteration == 0));
            currentBreadcrumbIndex--, iteration++) {
            //Logger.L(LoggerLevel.Notice, "Iteration " + iteration);
            //-------------------------------------------------------------------------------------
            // Step 2: We start with an initial search, ex Annie.dds
            //-------------------------------------------------------------------------------------
            string breadcrumb = inputPathBreadCrumbs[currentBreadcrumbIndex];
            //if (iteration == 0) searchPathTail = breadcrumb;
            //else searchPathTail = breadcrumb + "/" + searchPathTail;
            //Console.WriteLine("breadcrumb: " + breadcrumb + ", searchPathTail: " + searchPathTail);

            //-------------------------------------------------------------------------------------
            // Step 3: We look at our list of enumerated resources, and filter out resources that 
            //         do not end with Annie.dds.  If we end up with zero results, we fall back to 
            //         our previous nodes.
            //
            // Nov 17 - Flipped the foreach/ifs as an optimization
            //-------------------------------------------------------------------------------------
            if (iteration == 0) {
               //newMatches = new List<IDargonNode>(32);
               //int hash = ComputeResourceNameHash(breadcrumb);
//               foreach (var resource in initialCandidateNodes) {
//                  if (breadcrumb.Equals(resource.Name, StringComparison.OrdinalIgnoreCase))
//                     nextCandidateNodes.Add(resource);
//               }
               nextCandidateNodes.AddRange(initialCandidateNodes);
            } else {
               foreach (var candidate in currentCandidateNodes) {
                  // TODO: Optimize by only checking certain portions of the tail for equivalence.
                  //if (candidate.GetResourcePath().EndsWith(searchPathTail, StringComparison.OrdinalIgnoreCase)) 
                  var nodeOfInterest = candidate;
                  for (int i = 0; i < iteration; i++)
                     nodeOfInterest = nodeOfInterest.Parent;

                  if (nodeOfInterest != null && nodeOfInterest.NameEquals(breadcrumb))
                     nextCandidateNodes.Add(candidate);
               }
               //Console.WriteLine(iteration + " " + newMatches.Count);
            }

            if (nextCandidateNodes.Count == 0) {
               if (iteration == 0)
                  return new List<ReadableDargonNode>(); // We've resolved to nothing
               else {
                  doFinalize = true;
                  goto finalize; // yep.
               }
            } else if (nextCandidateNodes.Count == 1) {
               //We've filtered down to one candidate, so it's probably what we want.
               return nextCandidateNodes;
            }

            // Swap currentCandidateNodes with nextCandidateNodes, discard old set of candidates
            var temp = currentCandidateNodes;
            currentCandidateNodes = nextCandidateNodes;
            nextCandidateNodes = temp;
            nextCandidateNodes.Clear();

            if (iteration == inputPathBreadCrumbs.Length - 1)
               doFinalize = true;
            }
      finalize:
         if (doFinalize) {
            //Console.WriteLine("Do Finalize!");
            //----------------------------------------------------------------------------------
            // Riot has a tendency to drop champion names into random other champions' folders.  
            // We attempt to format and then tokenize our matching nodes' names.  We then look 
            // for those tokens in the paths of our nodes.  If we find a match path, then 
            // we're done (This works in cases such as Riot putting Garen files in Jarvan files 
            // for no reason at all)
            //----------------------------------------------------------------------------------
            List<ReadableDargonNode> resolvePath;
            if (TryFilterIncorrectlyNamedFiles(currentCandidateNodes, out resolvePath))
               return resolvePath;
         }
         return currentCandidateNodes; // new List<IDargonNode>();
      }

      private bool TryFilterIncorrectlyNamedFiles(List<ReadableDargonNode> input, out List<ReadableDargonNode> results)
      {
         // The precondition for this method is that the input files all have the same name and that
         // we actually have some input files
         if ((from match in input
            select match.Name.ToLower()).Distinct().Count() != 1 ||
             input.Count == 0) {
            results = null;
            return false;
         }

         string duplicatedName = input.First().Name; // Every input node shares this same name
         string[] fileNameTokens = Util.ExtractFileNameTokens(duplicatedName).ToArray(); //space delimited name.

         //string s = duplicatedName + " =>";
         //foreach (var token in fileNameTokens) s += " " + token;
         //Console.WriteLine(s);

         var submatches = new List<ReadableDargonNode>();
         foreach (var match in input) {
            if (match.Parent == null)
               continue; //This shouldn't ever happen unless a user intentionally gives us a file named 0.0.0.25 lol

            string parentPath = match.Parent.GetPath();
            foreach (var token in fileNameTokens) {
               if (parentPath.IndexOf(token, StringComparison.OrdinalIgnoreCase) != -1) {
                  //Console.WriteLine("Got submatch " + token + " and " + match.GetResourcePath());
                  submatches.Add(match);
                  break;
               }
            }
         }
         if (submatches.Any()) {
            //Likely better.
            results = submatches;
            return true;
         } else {
            results = null;
            return false;
         }
      }
   }
}
