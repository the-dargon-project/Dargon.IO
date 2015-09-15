using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItzWarty.IO;

namespace Dargon.IO.Components {
   public interface DataStreamComponentFactory {
         DataStreamComponent CreateForFile(string path);
   }

   public class DataStreamComponentFactoryImpl : DataStreamComponentFactory {
      private readonly IStreamFactory streamFactory;

      public DataStreamComponentFactoryImpl(IStreamFactory streamFactory) {
         this.streamFactory = streamFactory;
      }

      public DataStreamComponent CreateForFile(string path) {
         return new FileSystemDataStreamComponentImpl(streamFactory, path);
      }
   }
}
