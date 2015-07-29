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
         this.isLoggingEnabled = GetBoolean(kResolverLoggingEnabledKey, true);
      }

      public bool IsLoggingEnabled => isLoggingEnabled;

      private bool GetBoolean(string key, bool defaultValue) {
         bool result;
         if (!bool.TryParse(systemState.Get(key, defaultValue.ToString()), out result)) {
            result = defaultValue;
         }
         return result;
      }
   }
}
