using System;
using System.Collections.Generic;

namespace Dargon.IO {
   internal static class DargonNodeUtilities {
      /// <summary>
      /// Treeifies the given collection, converting it into a resource tree.
      /// </summary>
      /// <returns></returns>
      public static void TreeifyInto<TCollectionEntry>(
         this IEnumerable<TCollectionEntry> leafEntries,
         WritableDargonNode root,
         Func<TCollectionEntry, string[]> getBreadcrumbs,
         Func<string, WritableDargonNode> createInnerNode,
         Func<TCollectionEntry, WritableDargonNode> createLeaf
      ) {
         foreach (var leafEntry in leafEntries) {
            var breadcrumbs = getBreadcrumbs(leafEntry);

            var currentNode = root;
            for (int i = 1; i < breadcrumbs.Length - 1; i++) {
               string nextDirectoryName = breadcrumbs[i];
               WritableDargonNode nextNode;
               if (!currentNode.TryGetChild(nextDirectoryName, out nextNode)) {
                  nextNode = createInnerNode(nextDirectoryName);
                  currentNode.AddChild(nextNode);
               }
               currentNode = nextNode;
            }
            currentNode.AddChild(createLeaf(leafEntry));
         }
      }
   }
}
