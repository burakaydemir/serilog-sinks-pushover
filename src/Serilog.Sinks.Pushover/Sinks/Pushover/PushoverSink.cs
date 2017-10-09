using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.Pushover.Sinks.Pushover
{
    public class PushoverSink : ILogEventSink
    {
        private readonly ITextFormatter _messageFormatter;
        private readonly ITextFormatter _titleFormatter;
        private readonly string _apiUri;
        private readonly string _token;
        private readonly string _userOrGroupKey;
        private readonly string[] _devices;
        private readonly LogEventLevel _logEventLevel;
        private readonly LoggingLevelSwitch _loggingLevelSwitch;

        public PushoverSink(ITextFormatter titleFormatter, ITextFormatter messageFormatter, string apiUri, string token, string userOrGroupKey, string[] devices, LogEventLevel logEventLevel, LoggingLevelSwitch loggingLevelSwitch)
        {
            _titleFormatter = titleFormatter;
            _messageFormatter = messageFormatter;
            _apiUri = apiUri;
            _token = token;
            _userOrGroupKey = userOrGroupKey;
            _devices = devices;
            _logEventLevel = logEventLevel;
            _loggingLevelSwitch = loggingLevelSwitch;
        }

        /// <summary>
        /// Emit the provided log event to the sink.
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit(LogEvent logEvent)
        {
            using (var titleBuffer = new StringWriter())
            using (var messageBuffer = new StringWriter())
            {
                _titleFormatter.Format(logEvent, titleBuffer);
                _messageFormatter.Format(logEvent, messageBuffer);

                try
                {
                    var parameters = new NameValueCollection {
                        { "token", _token },
                        { "user", _userOrGroupKey },
                        { "title", _titleFormatter.ToString() },
                        { "message", _messageFormatter.ToString() },
                        { "device", string.Join(",", _devices) }
                    };

                    byte[] response;
                    using (var client = new WebClient())
                    {
                        response = client.UploadValues(_apiUri, parameters);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
