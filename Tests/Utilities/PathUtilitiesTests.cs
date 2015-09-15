using System.Linq;
using NMockito;
using Xunit;

namespace Dargon.IO.Utilities {
   public class PathUtilitiesTests : NMockitoInstance {
      [Fact]
      public void IsDelimiter_Tests() {
         AssertTrue(PathUtilities.IsDelimiter('/'));
         AssertTrue(PathUtilities.IsDelimiter('\\'));

         AssertFalse(PathUtilities.IsDelimiter('a'));
         AssertFalse(PathUtilities.IsDelimiter('#'));
      }

      [Fact]
      public void GetPathBreadCrumbs_UnixStylePath_Tests() {
         AssertTrue(new[] { "", "one", "two" }.SequenceEqual(PathUtilities.GetPathBreadCrumbs("/one/two")));
      }

      [Fact]
      public void GetPathBreadCrumbs_WindowsStylePath_Tests() {
         AssertTrue(new[] { "c:", "one", "two" }.SequenceEqual(PathUtilities.GetPathBreadCrumbs("c:/one/two")));
      }
   }
}