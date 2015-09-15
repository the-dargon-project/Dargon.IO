using ItzWarty.IO;

namespace Dargon.IO.Components {
   public interface DataStreamComponent {
      IStream CreateRead();
   }
}
