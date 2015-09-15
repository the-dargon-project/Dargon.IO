using System.Collections.Generic;

namespace Dargon.IO {
   public interface DargonNodeFactoryExtensions {
      WritableDargonNode CreateFromBreadcrumbPath(DargonNodeFactory nodeFactory, IReadOnlyList<string> breadcrumbs);
   }
}