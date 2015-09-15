namespace Dargon.IO {
   public interface DargonNodeFactory {
      WritableDargonNode Create(string name);
   }

   public class DargonNodeFactoryImpl : DargonNodeFactory {
      public WritableDargonNode Create(string name) => new MutableDargonNodeImpl(name);
   }
}