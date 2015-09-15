using Dargon.IO.Components;
using Dargon.Ryu;
using ItzWarty.IO;

namespace Dargon.IO.Drive {
   public static class DargonNodeFactoryDriveExtensions {
      private static IStreamFactory streamFactory;
      private static DataStreamComponentFactory dataStreamComponentFactory;
      private static DriveTreeImporter importer;

      public static void Initialize(RyuContainer ryu) {
         streamFactory = ryu.Get<IStreamFactory>();
         dataStreamComponentFactory = ryu.Get<DataStreamComponentFactory>();
         importer = ryu.Get<DriveTreeImporter>();
      }

      public static WritableDargonNode CreateDriveDirectoryNode(this DargonNodeFactory nodeFactory, string directoryName) {
         return nodeFactory.Create(directoryName);
      }

      public static WritableDargonNode CreateDriveFileNode(this DargonNodeFactory nodeFactory, IFileInfo fileInfo) {
         var node = nodeFactory.Create(fileInfo.Name);
         node.AddComponent(dataStreamComponentFactory.CreateForFile(fileInfo.FullName));
         return node;
      }

      public static WritableDargonNode ImportDirectoryAndParents(this DargonNodeFactory nodeFactory, string directory) {
         return importer.ImportDirectoryAndParents(nodeFactory, directory);
      }

      public static WritableDargonNode ImportFileTree(this DargonNodeFactory nodeFactory, string directory) {
         return importer.ImportFileTree(nodeFactory, directory);
      }
   }
}