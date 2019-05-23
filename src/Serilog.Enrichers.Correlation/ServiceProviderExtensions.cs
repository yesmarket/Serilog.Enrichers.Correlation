using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Enrichers.Correlation
{
   public static class ServiceProviderExtensions
   {
      public static void UseDiagnosticObserver<TObserver>(this IServiceProvider serviceProvider)
         where TObserver : IObserver<DiagnosticListener>
      {
         if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

         var diagnosticObservers = serviceProvider.GetServices<TObserver>();
         foreach (var diagnosticObserver in diagnosticObservers)
            DiagnosticListener.AllListeners.Subscribe(diagnosticObserver);
      }
   }
}
