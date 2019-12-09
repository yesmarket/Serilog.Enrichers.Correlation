# Serilog.Enrichers.Correlation

Use this to enrich logs with a Correlation Id

Configure serilog is as follows;
```json
"serilog": {
   "minimumLevel": { },
   "writeTo": [ ],
   "enrich": [ "WithCorrelation" ]
}
```

There is also a fix in this libarey for a [known issue](https://github.com/aspnet/AspNetCore/issues/5144) with ASP.NET core 2.2.0, which results in the correlation is not being set. To use the fix, do the following in your Startup.cs:
```
public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, IApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
{
   DiagnosticListener.AllListeners.Subscribe(new CorrelationIdObserver());
}
```

To install from nuget;
```bash
Install-Package Serilog.Enrichers.Correlation
```
