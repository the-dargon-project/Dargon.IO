using System.Collections.Generic;

namespace Dargon.IO {
   public interface ReadableDargonNode {
      string Name { get; }
      ReadableDargonNode Parent { get; }
      IReadOnlyCollection<ReadableDargonNode> Children { get; }

      T GetComponentOrNull<T>();
      bool TryGetChild(string name, out ReadableDargonNode child);
   }
}
