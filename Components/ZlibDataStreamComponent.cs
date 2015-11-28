using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zlib;
using ItzWarty.IO;

namespace Dargon.IO.Components {
   public class ZlibDataStreamComponent : DataStreamComponent {
      private readonly IStreamFactory streamFactory;
      private readonly DataStreamComponent innerComponent;

      public ZlibDataStreamComponent(IStreamFactory streamFactory, DataStreamComponent innerComponent) {
         this.innerComponent = innerComponent;
         this.streamFactory = streamFactory;
      }

      public IStream CreateRead() {
         using (var compressedStream = innerComponent.CreateRead()) {
            var decompressedStream = streamFactory.CreateMemoryStream();
            using (var decompressor = new ZlibStream(decompressedStream.__Stream, CompressionMode.Decompress)) {
               compressedStream.__Stream.CopyTo(decompressor);
            }
            return decompressedStream;
         }
      }
   }
}
