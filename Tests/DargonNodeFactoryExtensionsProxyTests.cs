using Dargon.Ryu;
using NMockito;
using Xunit;

namespace Dargon.IO {
   public class DargonNodeFactoryExtensionsProxyTests : NMockitoInstance {
      [Mock] private readonly DargonNodeFactoryExtensions nodeFactoryExtensions = null;
      [Mock] private readonly DargonNodeFactory nodeFactory = null;

      public DargonNodeFactoryExtensionsProxyTests() {
         var ryu = CreateMock<RyuContainer>();
         When(ryu.Get<DargonNodeFactoryExtensions>()).ThenReturn(nodeFactoryExtensions);
         DargonNodeFactoryExtensionsProxy.Initialize(ryu);
         ClearInteractions();
      }

      [Fact]
      public void CreateFromBreadcrumbPath_DelegatesToInstance_Test() {
         var expectedResult = CreateMock<WritableDargonNode>();
         var breadcrumbs = CreatePlaceholder<string[]>();

         When(nodeFactoryExtensions.CreateFromBreadcrumbPath(nodeFactory, breadcrumbs)).ThenReturn(expectedResult);

         var actualResult = nodeFactory.CreateFromBreadcrumbPath(breadcrumbs);

         Verify(nodeFactoryExtensions).CreateFromBreadcrumbPath(nodeFactory, breadcrumbs);
         VerifyNoMoreInteractions();

         AssertEquals(expectedResult, actualResult);
      }
   }
}