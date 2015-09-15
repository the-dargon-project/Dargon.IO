using Dargon.IO.Utilities;
using ItzWarty;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dargon.IO {
   public static class DargonNodeExtensions {
      public static ReadableDargonNode GetRoot(this ReadableDargonNode node) {
         node.ThrowIfNull(nameof(node));
         while (node.Parent != null) {
            node = node.Parent;
         }
         return node;
      }

      public static WritableDargonNode GetRoot(this WritableDargonNode node) {
         return (WritableDargonNode)GetRoot((ReadableDargonNode)node);
      }

      public static ReadableDargonNode GetChild(this ReadableDargonNode node, string name) {
         node.ThrowIfNull(nameof(node));
         ReadableDargonNode result;
         if (!node.TryGetChild(name, out result)) {
            throw new KeyNotFoundException("could not find child of the given name");
         }
         return result;
      }

      public static WritableDargonNode GetChild(this WritableDargonNode node, string name) {
         return (WritableDargonNode)GetChild((ReadableDargonNode)node, name);
      }

      public static ReadableDargonNode GetChildOrNull(this ReadableDargonNode node, string name) {
         node.ThrowIfNull(nameof(node));
         ReadableDargonNode result;
         node.TryGetChild(name, out result);
         return result;
      }

      public static WritableDargonNode GetChildOrNull(this WritableDargonNode node, string name) {
         return (WritableDargonNode)GetChildOrNull((ReadableDargonNode)node, name);
      }

      public static ReadableDargonNode GetRelativeOrNull(this ReadableDargonNode node, string relativePath) {
         return node.GetRelativeOrNull<ReadableDargonNode>(relativePath);
      }

      public static WritableDargonNode GetRelativeOrNull(this WritableDargonNode node, string relativePath) {
         return node.GetRelativeOrNull<WritableDargonNode>(relativePath);
      }

      public static TNode GetRelativeOrNull<TNode>(this ReadableDargonNode node, string relativePath)
         where TNode : ReadableDargonNode {
         var currentNode = node;
         var breadcrumbs = PathUtilities.GetPathBreadCrumbs(relativePath);
         if (breadcrumbs.Any()) {
            if (breadcrumbs.First() == "") {
               currentNode = currentNode.GetRoot();
            }
            foreach (var breadcrumb in breadcrumbs) {
               if (breadcrumb == "") continue;
               if (!currentNode.TryGetChild(breadcrumb, out currentNode)) {
                  return default(TNode);
               }
            }
         }
         return (TNode)currentNode;
      }

      public static bool NameEquals(this ReadableDargonNode node, string name) {
         node.ThrowIfNull(nameof(node));
         name.ThrowIfNull(nameof(name));
         return node.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
      }

      public static string GetPath(this ReadableDargonNode node, string delimiter = "/") {
         node.ThrowIfNull(nameof(node));
         delimiter.ThrowIfNull(nameof(delimiter));
         var s = new Stack<string>();
         while (node != null) {
            s.Push(node.Name);
            node = node.Parent;
         }
         return s.Join(delimiter);
      }

      public static IReadOnlyList<ReadableDargonNode> GetLeaves(this ReadableDargonNode start) {
         var results = new List<ReadableDargonNode>();
         var s = new Stack<ReadableDargonNode>();
         s.Push(start);
         while (s.Any()) {
            var node = s.Pop();
            var children = node.Children;
            if (children.None()) {
               results.Add(node);
            } else {
               children.ForEach(s.Push);
            }
         }
         return results;
      }
   }
}
