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

        public static LoggerConfiguration Pushover(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            string token,
            string userOrGroupKey,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Warning,
            string outputTitleTemplate = DefaultPushoverTitleTemplate,
            string outputMessageTemplate = DefaultPushoverMessageTemplate,
            string apiUri = DefaultPushoverUri,
            string[] devices = null,
            PushoverMessagePriority pushoverMessagePriority = PushoverMessagePriority.HighPriority,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (loggerSinkConfiguration == null) throw new ArgumentNullException(nameof(loggerSinkConfiguration));
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrWhiteSpace(userOrGroupKey)) throw new ArgumentNullException(nameof(userOrGroupKey));
            if (outputTitleTemplate == null) throw new ArgumentNullException(nameof(outputTitleTemplate));
            if (outputMessageTemplate == null) throw new ArgumentNullException(nameof(outputMessageTemplate));

            var titleFormatter = new MessageTemplateTextFormatter(outputTitleTemplate, formatProvider);
            var messageFormatter = new MessageTemplateTextFormatter(outputMessageTemplate, formatProvider);

            return loggerSinkConfiguration.Sink(new Sinks.Pushover.PushoverSink(titleFormatter, messageFormatter, apiUri, token, userOrGroupKey, devices, pushoverMessagePriority, restrictedToMinimumLevel));
        }
    }
}