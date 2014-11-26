using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.IO {
   internal static class DargonNodeUtilities {
      /// <summary>
      /// Treeifies the given collection, converting it into a resource tree.
      /// </summary>
      /// <returns></returns>
      public static TResultNode Treeify<TResultNode, TCollectionEntry, TBreadcrumb>(
         this IEnumerable<TCollectionEntry> collection,
         TResultNode root,
         Func<TCollectionEntry, string[]> getBreadcrumbs,
         Func<string, IWritableDargonNode> createInnerNode,
         Func<TCollectionEntry, IWritableDargonNode> createLeaf
      ) where TResultNode : IWritableDargonNode {
         foreach (var entry in collection) {
            var breadcrumbs = getBreadcrumbs(entry);

            //Get or Create the tree levels up to our content file.
            IWritableDargonNode currentNode = root;
            if (!root.Name.Equals(breadcrumbs[0], StringComparison.OrdinalIgnoreCase))
               throw new ArgumentException("Not all nodes were from the same root/drive!");

            for (int i = 1; i < breadcrumbs.Length - 1; i++) {
               string nextDirectoryName = breadcrumbs[i];
               var childMatches = from child in currentNode.Children
                                  where String.Equals(child.Name, nextDirectoryName, StringComparison.Ordinal)
                                  select child;

               var match = childMatches.FirstOrDefault();
               if (match == null) {
                  //Create a new directory
                  var newNode = createInnerNode(nextDirectoryName);
                  currentNode.AddChild(newNode);
                  currentNode = newNode;
               } else {
                  currentNode = match;
               }
            }
            currentNode.AddChild(createLeaf(entry));
         }
         return root;
      }
   }
}
