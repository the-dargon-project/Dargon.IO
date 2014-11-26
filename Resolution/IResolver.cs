using System.Collections.Generic;

namespace Dargon.IO.Resolution {
   public interface IResolver {
      IReadOnlyList<IReadableDargonNode> Resolve(string inputPath, string hintPath = null);
   }
}