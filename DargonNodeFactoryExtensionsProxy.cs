using Dargon.Ryu;

namespace Dargon.IO {
   public static class DargonNodeFactoryExtensionsProxy {
      private static DargonNodeFactoryExtensions instance;

      public static void Initialize(RyuContainer ryu) {
         instance = ryu.Get<DargonNodeFactoryExtensions>();
      }

      public static WritableDargonNode CreateFromBreadcrumbPath(this DargonNodeFactory nodeFactory, string[] breadcrumbs) {
         return instance.CreateFromBreadcrumbPath(nodeFactory, breadcrumbs);
      }
   }
}