using System;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Pushover.Sinks.Pushover;

namespace Serilog.Sinks.Pushover
{
    public static class LoggerConfigurationPushoverExtensions
    {
        const string DefaultPushoverUri = "https://api.pushover.net/1/messages.json";
        const string DefaultPushoverTitleTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}";
        const string DefaultPushoverMessageTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration PushoverSink(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTitleTemplate = DefaultPushoverTitleTemplate,
            string outputMessageTemplate = DefaultPushoverMessageTemplate,
            string apiUri = DefaultPushoverUri,
            string token = null,
            string userOrGroupKey = null,
            string[] devices = null,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (loggerSinkConfiguration == null) throw new ArgumentNullException(nameof(loggerSinkConfiguration));
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (userOrGroupKey == null) throw new ArgumentNullException(nameof(userOrGroupKey));
            if (outputMessageTemplate == null) throw new ArgumentNullException(nameof(outputMessageTemplate));

            var titleFormatter = new MessageTemplateTextFormatter(outputTitleTemplate, formatProvider);
            var messageFormatter = new MessageTemplateTextFormatter(outputMessageTemplate, formatProvider);

            return loggerSinkConfiguration.Sink(new PushoverSink(titleFormatter, messageFormatter, apiUri, token, userOrGroupKey, devices, restrictedToMinimumLevel, levelSwitch));
        }
    }
}