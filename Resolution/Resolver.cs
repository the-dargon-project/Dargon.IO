using System.Collections.Generic;

namespace Dargon.IO.Resolution {
   public interface Resolver {
      IReadOnlyList<ReadableDargonNode> Resolve(string inputPath, string hintPath = null);
   }
}