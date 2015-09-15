using System.Collections.Generic;

namespace Dargon.IO {
   public class DargonNodeFactoryExtensionsImpl : DargonNodeFactoryExtensions {
      public WritableDargonNode CreateFromBreadcrumbPath(DargonNodeFactory nodeFactory, IReadOnlyList<string> breadcrumbs) {
         WritableDargonNode currentNode = null;
         foreach (var breadcrumb in breadcrumbs) {
            var nextNode = nodeFactory.Create(breadcrumb);
            currentNode?.AddChild(nextNode);
            currentNode = nextNode;
         }
         return currentNode;
      }
   }
}