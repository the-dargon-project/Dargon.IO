using ItzWarty.Collections;
using System;
using System.Collections.Generic;

namespace Dargon.IO {
   public interface WritableDargonNode : ReadableDargonNode {
      new WritableDargonNode Parent { get; set; }
      new IReadOnlyCollection<WritableDargonNode> Children { get; }


      bool AddChild(WritableDargonNode node);
      bool RemoveChild(WritableDargonNode node);
      bool TryGetChild(string name, out WritableDargonNode child);
      void AddComponent<TInterface>(TInterface component);
   }
}
