using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.IO.Resolution {
   public interface ResolverConfiguration {
      bool IsLoggingEnabled { get; }
   }

   public class ResolverConfigurationImpl : ResolverConfiguration {
      private const string kResolverLoggingEnabledKey = "dargon.io.resolver.logging_enabled";
      private readonly SystemState systemState;
      private readonly bool isLoggingEnabled;

      public ResolverConfigurationImpl(SystemState systemState) {
         this.systemState = systemState;
         this.isLoggingEnabled = systemState.Get(kResolverLoggingEnabledKey, true);
      }

      public bool IsLoggingEnabled => isLoggingEnabled;
   }
}
