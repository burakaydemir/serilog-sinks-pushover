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


### XML `<appSettings>` configuration

To use the pushover sink with the [Serilog.Settings.AppSettings](https://github.com/serilog/serilog-settings-appsettings) package, first install that package if you haven't already done so:

```powershell
Install-Package Serilog.Settings.AppSettings
```

Instead of configuring the logger in code, call `ReadFrom.AppSettings()`:

```csharp
var log = new LoggerConfiguration()
    .ReadFrom.AppSettings()
    .CreateLogger();
```

In your application's `App.config` or `Web.config` file, specify the pushover sink assembly under the `<appSettings>` node:

```xml
<configuration>
  <appSettings>
    <add key="serilog:using:Pushover" value="Serilog.Sinks.Pushover" />
    <add key="serilog:write-to:Pushover.token" value="xxx" />
    <add key="serilog:write-to:Pushover.userOrGroupKey" value="xxx" />
    <add key="serilog:write-to:Pushover.restrictedToMinimumLevel" value="Error" />
    <add key="serilog:write-to:Pushover.outputTitleTemplate" value="[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}" />
    <add key="serilog:write-to:Pushover.outputMessageTemplate" value="[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}" />
```


