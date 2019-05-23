using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Serilog.Enrichers.Correlation
{
   public static class ServiceProviderExtensions
   {
      public static void UseDiagnosticObserver(this IServiceProvider serviceProvider)
      {
         if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

         var diagnosticObservers = serviceProvider.GetServices<IObserver<DiagnosticListener>>();
         foreach (var diagnosticObserver in diagnosticObservers)
            DiagnosticListener.AllListeners.Subscribe(diagnosticObserver);
      }
   }
}
