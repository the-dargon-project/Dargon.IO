using ItzWarty;
using ItzWarty.Comparers;
using System;
using System.Collections.Generic;

namespace Dargon.IO {
   public class MutableDargonNodeImpl : WritableDargonNode {
      private readonly IDictionary<string, WritableDargonNode> childrenByName = new Dictionary<string, WritableDargonNode>(new CaseInsensitiveStringEqualityComparer());
      private readonly Dictionary<Type, object> componentsByType = new Dictionary<Type, object>();
      private WritableDargonNode parent;

      public MutableDargonNodeImpl(string name) {
         this.Name = name;
      }

      public string Name { get; set; }

      ReadableDargonNode ReadableDargonNode.Parent => parent;
      public WritableDargonNode Parent { get { return parent; } set { SetParent(value); } }

      IReadOnlyCollection<ReadableDargonNode> ReadableDargonNode.Children => Children;
      public IReadOnlyCollection<WritableDargonNode> Children => (IReadOnlyCollection<WritableDargonNode>)childrenByName.Values;

      public void AddComponent<TComponent>(TComponent instance) => componentsByType.Add(typeof(TComponent), instance);

      public T GetComponentOrNull<T>() => (T)componentsByType.GetValueOrDefault(typeof(T));

      public bool AddChild(WritableDargonNode node) {
         if (childrenByName.ContainsKey(node.Name)) {
            return false;
         } else {
            childrenByName.Add(node.Name, node);
            node.Parent = this;
            return true;
         }
      }

      public bool RemoveChild(WritableDargonNode node) => childrenByName.Remove(node.Name.PairValue(node));

      public bool TryGetChild(string name, out WritableDargonNode child) => childrenByName.TryGetValue(name, out child);

      public bool TryGetChild(string name, out ReadableDargonNode child) {
         WritableDargonNode node;
         var succeeded = TryGetChild(name, out node);
         child = node;
         return succeeded;
      }

      private void SetParent(WritableDargonNode newParent) {
         var oldParent = parent;
         if (newParent == oldParent) return;

         parent = newParent;
         oldParent?.RemoveChild(this);
         newParent?.AddChild(this);
      }
   }
}
