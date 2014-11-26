using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItzWarty.IO;

namespace Dargon.IO.Drive {
   public class DriveNodeFactory : IDriveNodeFactory {
      private readonly IStreamFactory streamFactory;

      public DriveNodeFactory(IStreamFactory streamFactory) {
         this.streamFactory = streamFactory;
      }

      public IWritableDargonNode CreateFromDirectory(string directory) {
         // Create the nodes all the way up to the Directory node.
         var directoryInfo = new DirectoryInfo(directory);
         DriveDirectoryNode baseNode = null;
         var root = new List<string> { directoryInfo.FullName }.Treeify<DriveDirectoryNode, string, string>(
            new DriveDirectoryNode(directoryInfo.FullName.Split('\\', '/').First()),
            (s) => s.Split('\\', '/'),
            (s) => new DriveDirectoryNode(s),
            (crumbs) => baseNode = new DriveDirectoryNode(directoryInfo.Name)
         );
         directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).Treeify<DriveDirectoryNode, FileInfo, string>(
            root,
            (fileInfo) => fileInfo.FullName.Split('\\', '/'),
            (crumb) => new DriveDirectoryNode(crumb),
            (fileInfo) => new DriveFileNode(fileInfo.Name, streamFactory)
         );
         return baseNode;
      }
   }
}
