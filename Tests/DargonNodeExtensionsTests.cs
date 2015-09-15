using System;
using System.Collections.Generic;
using NMockito;
using Xunit;

namespace Dargon.IO {
   public class DargonNodeExtensionsTests : NMockitoInstance {
      private readonly WritableDargonNode a = new MutableDargonNodeImpl("a");
      private readonly WritableDargonNode b = new MutableDargonNodeImpl("b");
      private readonly WritableDargonNode c = new MutableDargonNodeImpl("c");
      private readonly WritableDargonNode d = new MutableDargonNodeImpl("d");
      private readonly WritableDargonNode e = new MutableDargonNodeImpl("e");
      private readonly WritableDargonNode f = new MutableDargonNodeImpl("f");
      private readonly WritableDargonNode g = new MutableDargonNodeImpl("g");
      private readonly WritableDargonNode nullNode = null;

      public DargonNodeExtensionsTests() {
         a.AddChild(b);
         b.AddChild(c);
         a.AddChild(d);
         d.AddChild(e);
         d.AddChild(f);
         d.AddChild(g);
      }

      [Fact]
      public void GetRoot_NullNode_ThrowsTest() {
         AssertThrows<ArgumentNullException>(() => nullNode.GetRoot());
      }

      [Fact]
      public void GetRoot_HappyPathTest() {
         AssertEquals(a, a.GetRoot());
         AssertEquals(a, b.GetRoot());
         AssertEquals(a, c.GetRoot());
         AssertEquals(a, d.GetRoot());
         AssertEquals(a, e.GetRoot());
         AssertEquals(a, f.GetRoot());
         AssertEquals(a, g.GetRoot());
      }

      [Fact]
      public void GetChild_DoesNotExist_ThrowsTest() {
         AssertThrows<KeyNotFoundException>(() => a.GetChild(g.Name));
      }

      [Fact]
      public void GetChild_NullNode_ThrowsTest() {
         AssertThrows<ArgumentNullException>(() => nullNode.GetChild(""));
      }

      [Fact]
      public void GetChild_HappyPathTest() {
         AssertEquals(b, a.GetChild(nameof(b)));
         AssertEquals(c, b.GetChild(nameof(c)));
         AssertEquals(d, a.GetChild(nameof(d)));
         AssertEquals(e, d.GetChild(nameof(e)));
         AssertEquals(f, d.GetChild(nameof(f)));
         AssertEquals(g, d.GetChild(nameof(g)));
      }

      [Fact]
      public void GetChildOrNull_NullNode_ThrowsTest() {
         AssertThrows<ArgumentNullException>(() => nullNode.GetChildOrNull(""));
      }

      [Fact]
      public void GetChildOrNull_HappyPathTest() {
         AssertEquals(b, a.GetChildOrNull(nameof(b)));
         AssertEquals(c, b.GetChildOrNull(nameof(c)));
         AssertEquals(d, a.GetChildOrNull(nameof(d)));

         AssertEquals(null, a.GetChildOrNull(nameof(g)));
         AssertEquals(null, b.GetChildOrNull(nameof(g)));
      }

      [Fact]
      public void GetRelativeOrNull_AbsolutePath_HappyPathTest() {
         AssertEquals(d, ((ReadableDargonNode)b).GetRelativeOrNull("/d"));
         AssertEquals(d, g.GetRelativeOrNull("/d"));

         AssertEquals(null, b.GetRelativeOrNull("/g/d"));
      }

      [Fact]
      public void NameEquals_HappyPathTest() {
         AssertTrue(a.NameEquals("A"));
         AssertTrue(a.NameEquals("a"));
         AssertFalse(a.NameEquals("b"));
      }

      [Fact]
      public void GetPath_HappyPathTest() {
         AssertEquals("a", a.GetPath());
         AssertEquals("a/b", b.GetPath());
         AssertEquals("a@@b@@c", c.GetPath("@@"));
         AssertEquals("a/d", d.GetPath());
         AssertEquals("a/d/e", e.GetPath());
         AssertEquals("a/d/f", f.GetPath());
         AssertEquals("a/d/g", g.GetPath());
      }

      [Fact]
      public void GetLeaves_HappyPathTest() {
         AssertTrue(new HashSet<ReadableDargonNode> { c, e, f, g }.SetEquals(a.GetLeaves()));
         AssertTrue(new HashSet<ReadableDargonNode> { c }.SetEquals(b.GetLeaves()));
         AssertTrue(new HashSet<ReadableDargonNode> { c }.SetEquals(c.GetLeaves()));
         AssertTrue(new HashSet<ReadableDargonNode> { e, f, g }.SetEquals(d.GetLeaves()));
         AssertTrue(new HashSet<ReadableDargonNode> { e }.SetEquals(e.GetLeaves()));
         AssertTrue(new HashSet<ReadableDargonNode> { f }.SetEquals(f.GetLeaves()));
         AssertTrue(new HashSet<ReadableDargonNode> { g }.SetEquals(g.GetLeaves()));
      }
   }
}