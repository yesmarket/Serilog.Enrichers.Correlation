using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace Serilog.Enrichers.Correlation
{
    public class CorrelationIdLogEventEnricher : ILogEventEnricher
    {
       public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
       {
          logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("CorrelationId", Trace.CorrelationManager.ActivityId));
       }
   }
}
