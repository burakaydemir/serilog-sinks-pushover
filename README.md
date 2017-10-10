# Serilog.Sinks.Pushover [![NuGet Version](http://img.shields.io/nuget/v/Serilog.Sinks.Pushover.svg?style=flat)](https://www.nuget.org/packages/Serilog.Sinks.Pushover/)

Sends log events using Pushover.

**Package** - [Serilog.Sinks.Pushover](http://nuget.org/packages/serilog.sinks.pushover)
| **Platforms** - .NET Standart

```csharp
var log = new LoggerConfiguration()
                .WriteTo.PushoverSink(ConfigurationManager.AppSettings["pushover:token"],
                                      ConfigurationManager.AppSettings["pushover:userOrGroupKey"],
                                      LogEventLevel.Error,
                                      pushoverMessagePriority: PushoverMessagePriority.HighPriority)
                .CreateLogger();
```
