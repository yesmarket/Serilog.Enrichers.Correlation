using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;
using OpenTracing.Util;

namespace Serilog.Enrichers.Correlation
{
   public sealed class CorrelationIdObserver : IObserver<DiagnosticListener>, IDisposable
   {
      private readonly List<IDisposable> _subscriptions = new List<IDisposable>();
      private bool _disposed;

      public void OnCompleted()
      {
         Dispose(true);
         _subscriptions.Clear();
      }

      public void OnError(Exception error)
      {
      }

      public void OnNext(DiagnosticListener value)
      {
         if (!new[] {"Microsoft.AspNetCore", "HttpHandlerDiagnosticListener"}.Contains(value.Name)) return;
         _subscriptions.Add(value.SubscribeWithAdapter(this));
      }

      [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")]
      public void OnHttpRequestInStart(HttpContext httpContext)
      {
         var headers = httpContext.Request.Headers;
         if (headers.TryGetValue("X-Correlation-ID", out var header))
            if (Guid.TryParse(header, out var correlationId))
            {
               Trace.CorrelationManager.ActivityId = correlationId;
               return;
            }

         Trace.CorrelationManager.ActivityId = GetCorrelationId();
      }

      private Guid GetCorrelationId()
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

      [DiagnosticName("System.Net.Http.HttpRequestOut.Start")]
      public void OnHttpRequestOutStart(HttpRequestMessage request)
      {
         request.Headers.Add("X-Correlation-ID", Trace.CorrelationManager.ActivityId.ToString());
      }

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      private void Dispose(bool disposing)
      {
         if (_disposed)
            return;

         if (disposing)
         {
            _subscriptions.ForEach(_ => _.Dispose());
         }

         _disposed = true;
      }
   }
}
