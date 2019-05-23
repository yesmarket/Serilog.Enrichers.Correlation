using System;
using System.Security.Cryptography;
using System.Text;
using OpenTracing.Util;

namespace Serilog.Enrichers.Correlation
{
   public class CorrelationIdResolver : ICorrelationIdResolver
   {
      public Guid GetCorrelationId()
      {
         var tracer = GlobalTracer.Instance;
         var traceId = tracer?.ActiveSpan?.Context?.TraceId;
         if (traceId == null) return new Guid();
         using (var md5 = MD5.Create())
         {
            var buffer = Encoding.Default.GetBytes(traceId);
            var hash = md5.ComputeHash(buffer);
            return new Guid(hash);
         }
      }
   }
}