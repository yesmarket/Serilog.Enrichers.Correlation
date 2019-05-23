using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Serilog.Enrichers.Correlation
{
   public static class ServiceCollectionExtensions
   {
      public static void AddDiagnosticObserver<TDiagnosticObserver>(this IServiceCollection services)
         where TDiagnosticObserver : class, IObserver<DiagnosticListener>
      {
         services.TryAddSingleton<ICorrelationIdResolver, CorrelationIdResolver>();
         services.TryAddEnumerable(ServiceDescriptor.Transient<IObserver<DiagnosticListener>, TDiagnosticObserver>());
      }
   }
}
