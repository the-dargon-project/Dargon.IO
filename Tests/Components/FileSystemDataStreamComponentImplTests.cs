using System.IO;
using ItzWarty.IO;
using NMockito;
using Xunit;

namespace Dargon.IO.Components {
   public class FileSystemDataStreamComponentImplTests : NMockitoInstance {
      private const string kFilePath = "FILE_PATH";

      [Mock] private readonly IStreamFactory streamFactory = null;

      private FileSystemDataStreamComponentImpl testObj;

      public FileSystemDataStreamComponentImplTests() {
         testObj = new FileSystemDataStreamComponentImpl(streamFactory, kFilePath);
      }

      [Fact]
      public void CreateRead_HappyPath_Test() {
         var fileStream = CreateMock<IFileStream>();

         When(streamFactory.CreateFileStream(kFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)).ThenReturn(fileStream);

         var resultStream = testObj.CreateRead();

         Verify(streamFactory).CreateFileStream(kFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
         VerifyNoMoreInteractions();

         AssertEquals(fileStream, resultStream);
      }
   }
}