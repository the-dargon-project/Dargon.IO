using Dargon.IO.Drive;

namespace Dargon.IO {
   public class DargonNodeFactory : IDargonNodeFactory {
      private readonly IDriveNodeFactory driveNodeFactory;

      public DargonNodeFactory(IDriveNodeFactory driveNodeFactory) {
         this.driveNodeFactory = driveNodeFactory;
      }

      public IWritableDargonNode CreateFromDirectory(string directory) {
         return driveNodeFactory.CreateFromDirectory(directory);
      }
   }
}
