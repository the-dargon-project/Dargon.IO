using System;

namespace Dargon.IO.Utilities {
   public static class PathUtilities {
      private static readonly char[] kPathDelimiters = { '/', '\\' };

      public static bool IsDelimiter(char c) => c == '/' || c == '\\';

      public static string[] GetPathBreadCrumbs(string path) => path.Split(kPathDelimiters);
   }
}
