using System.IO;
using ItzWarty.IO;

namespace Dargon.IO.Components {
   internal class FileSystemDataStreamComponentImpl : DataStreamComponent {
      private readonly IStreamFactory streamFactory;
      private readonly string filePath;

      public FileSystemDataStreamComponentImpl(IStreamFactory streamFactory, string filePath) {
         this.streamFactory = streamFactory;
         this.filePath = filePath;
      }

      public IStream CreateRead() {
         return streamFactory.CreateFileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
      }
   }
}