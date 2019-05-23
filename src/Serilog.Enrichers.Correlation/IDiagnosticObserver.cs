using System;
using System.Diagnostics;

namespace Serilog.Enrichers.Correlation
{
   public interface IDiagnosticObserver : IObserver<DiagnosticListener>
   {
   }
}