using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Serilog.Enrichers.Correlation
{
   public static class ServiceCollectionExtensions
   {
      public static void AddDiagnosticObserver<TDiagnosticObserver>(this IServiceCollection services)
         where TDiagnosticObserver : class, IDiagnosticObserver
      {
         services.TryAddSingleton<ICorrelationIdResolver, CorrelationIdResolver>();
         services.TryAddEnumerable(ServiceDescriptor.Transient<IDiagnosticObserver, TDiagnosticObserver>());
      }
   }
}