using NLog;

namespace Dargon.IO.Resolution {
   public interface ResolverFactory {
      Resolver CreateForNode(ReadableDargonNode node);
   }

   public class ResolverFactoryImpl : ResolverFactory {
      private readonly Logger defaultResolverLogger = LogManager.GetLogger(typeof(DefaultResolver).FullName);
      private readonly Logger nullLogger = LogManager.CreateNullLogger();
      private readonly ResolverConfiguration configuration;

      public ResolverFactoryImpl(ResolverConfiguration configuration) {
         this.configuration = configuration;
      }

      public Resolver CreateForNode(ReadableDargonNode node) {
         var logger = configuration.IsLoggingEnabled ? defaultResolverLogger : nullLogger;
         return new DefaultResolver(logger, node);
      }
   }
}
