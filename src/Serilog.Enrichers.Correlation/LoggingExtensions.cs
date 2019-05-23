using System;
using Serilog.Configuration;

namespace Serilog.Enrichers.Correlation
{
   public static class LoggingExtensions
   {
      public static LoggerConfiguration WithCorrelationId(this LoggerEnrichmentConfiguration enrich)
      {
         if (enrich == null)
            throw new ArgumentNullException(nameof(enrich));

         return enrich.With<CorrelationIdLogEventEnricher>();
      }
   }
}
