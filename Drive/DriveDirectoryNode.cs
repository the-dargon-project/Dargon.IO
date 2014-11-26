using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.IO.Drive {
   public class DriveDirectoryNode : DargonNode {
      public DriveDirectoryNode(string name) : base(name) {
         if (name == null) {
            throw new ArgumentNullException("name");
         }
      }
      
   }
}
