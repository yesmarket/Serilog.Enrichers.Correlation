using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;

namespace Serilog.Enrichers.Correlation
{
   public class CorrelationIdObserver : IObserver<DiagnosticListener>
   {
      private readonly ICorrelationIdResolver _correlationIdResolver;
      private readonly List<IDisposable> _subscriptions = new List<IDisposable>();

      public CorrelationIdObserver(ICorrelationIdResolver correlationIdResolver)
      {
         _correlationIdResolver = correlationIdResolver;
      }

      public void OnCompleted()
      {
         _subscriptions.ForEach(x => x.Dispose());
         _subscriptions.Clear();
      }

      public void OnError(Exception error)
      {
      }

      public void OnNext(DiagnosticListener value)
      {
         if (!new[] {"Microsoft.AspNetCore", "HttpHandlerDiagnosticListener"}.Contains(value.Name)) return;

         var subscription = value.SubscribeWithAdapter(this);
         _subscriptions.Add(subscription);
      }

      [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn")]
      public void OnHttpRequestIn()
      {
      }

      [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")]
      public void OnHttpRequestInStart(HttpContext httpContext)
      {
         var headers = httpContext.Request.Headers;
         if (headers.TryGetValue("X-Correlation-ID", out var header))
            if (Guid.TryParse(header, out var correlationId))
            {
               AsyncLocal.CorrelationId.Current.Value = correlationId;
               return;
            }

         AsyncLocal.CorrelationId.Current.Value = _correlationIdResolver.GetCorrelationId();
      }

      [DiagnosticName("System.Net.Http.HttpRequestOut")]
      public void OnHttpRequestOut()
      {
      }

      [DiagnosticName("System.Net.Http.HttpRequestOut.Start")]
      public void OnHttpRequestOutStart(HttpRequestMessage request)
      {
         var correlationId = AsyncLocal.CorrelationId.Current.Value.ToString();
         request.Headers.Add("X-Correlation-ID", correlationId);
      }
   }
}
