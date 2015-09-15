using NMockito;

namespace Dargon.IO {
   public class DargonNodeFactoryImplTests : NMockitoInstance {
      private readonly DargonNodeFactoryImpl testObj;

      public DargonNodeFactoryImplTests() {
         this.testObj = new DargonNodeFactoryImpl();
      }

      public void Create_Test() {
         var name = CreatePlaceholder<string>();
         var node = testObj.Create(name);
         AssertEquals(name, node.Name);
      }
   }
}