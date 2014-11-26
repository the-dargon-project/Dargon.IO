using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItzWarty.IO;

namespace Dargon.IO.Drive {
   public class DriveFileNode : DargonNode {
      public DriveFileNode(string name, IStreamFactory streamFactory) : base(name) {
         if (name == null) {
            throw new ArgumentNullException("name");
         }

         AddComponent(typeof(IDataSourceComponent), new DataSourceComponent(this, streamFactory));
      }

      public class DataSourceComponent : IDataSourceComponent {
         private readonly DriveFileNode driveFileNode;
         private readonly IStreamFactory streamFactory;

         public DataSourceComponent(DriveFileNode driveFileNode, IStreamFactory streamFactory) {
            this.driveFileNode = driveFileNode;
            this.streamFactory = streamFactory;
         }

         public IStream GetDataStream() {
            return streamFactory.CreateFileStream(driveFileNode.GetPath(), FileMode.Open, FileAccess.Read, FileShare.Read);
         }
      }
   }
}
