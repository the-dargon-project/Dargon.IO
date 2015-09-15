using NMockito;
using Xunit;

namespace Dargon.IO {
   public class DargonNodeFactoryExtensionsImplTests : NMockitoInstance {
      [Mock] private readonly DargonNodeFactory nodeFactory = null;

      private readonly DargonNodeFactoryExtensionsImpl testObj;

      public DargonNodeFactoryExtensionsImplTests() {
         this.testObj = new DargonNodeFactoryExtensionsImpl();
      }

      [Fact]
      public void CreateFromBreadcrumbPath_Test() {
         var a = CreateMock<WritableDargonNode>();
         var b = CreateMock<WritableDargonNode>();
         var c = CreateMock<WritableDargonNode>();

         When(nodeFactory.Create(nameof(a))).ThenReturn(a);
         When(nodeFactory.Create(nameof(b))).ThenReturn(b);
         When(nodeFactory.Create(nameof(c))).ThenReturn(c);

         var result = testObj.CreateFromBreadcrumbPath(nodeFactory, new[] { nameof(a), nameof(b), nameof(c) });

         Verify(nodeFactory).Create(nameof(a));
         Verify(nodeFactory).Create(nameof(b));
         Verify(nodeFactory).Create(nameof(c));

         Verify(a).AddChild(b);
         Verify(b).AddChild(c);
         VerifyNoMoreInteractions();

         AssertEquals(c, result);
      }
   }
}