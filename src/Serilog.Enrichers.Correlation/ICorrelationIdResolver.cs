using System;

namespace Serilog.Enrichers.Correlation
{
   public interface ICorrelationIdResolver
   {
      Guid GetCorrelationId();
   }
}