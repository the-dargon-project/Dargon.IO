using ItzWarty.IO;
using System.IO;
using System.Linq;
using Dargon.IO.Utilities;

namespace Dargon.IO.Drive {
   public interface DriveTreeImporter {
      WritableDargonNode ImportDirectoryAndParents(DargonNodeFactory nodeFactory, string directory);
      WritableDargonNode ImportFileTree(DargonNodeFactory nodeFactory, string directory);
   }

   public class DriveTreeImporterImpl : DriveTreeImporter {
      private readonly IFileSystemProxy fileSystemProxy;

      public DriveTreeImporterImpl(IFileSystemProxy fileSystemProxy) {
         this.fileSystemProxy = fileSystemProxy;
      }

      public WritableDargonNode ImportDirectoryAndParents(DargonNodeFactory nodeFactory, string directory) {
         // Create the nodes all the way up to the Directory node.
         var directoryInfo = fileSystemProxy.GetDirectoryInfo(directory);
         var breadcrumbs = PathUtilities.GetPathBreadCrumbs(directoryInfo.FullName);

         return nodeFactory.CreateFromBreadcrumbPath(breadcrumbs);
      }

      public WritableDargonNode ImportFileTree(DargonNodeFactory nodeFactory, string directory) {
         var baseNode = nodeFactory.ImportDirectoryAndParents(directory);

         var directoryInfo = fileSystemProxy.GetDirectoryInfo(directory);
         var directoryFullName = directoryInfo.FullName;
         var fileInfos = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories);
         fileInfos.TreeifyInto(
            baseNode,
            (fileInfo) => PathUtilities.GetPathBreadCrumbs(fileInfo.FullName.Substring(directoryFullName.Length + 1)),
            (crumb) => nodeFactory.CreateDriveDirectoryNode(crumb),
            (fileInfo) => nodeFactory.CreateDriveFileNode(fileInfo)
         );
         return baseNode;
      }
   }
}
