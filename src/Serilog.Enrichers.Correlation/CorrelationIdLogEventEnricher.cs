using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers.Correlation
{
    public class CorrelationIdLogEventEnricher : ILogEventEnricher
    {
       public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
       {
          logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("CorrelationId", AsyncLocal.CorrelationId.Current.Value));
       }
   }
}
