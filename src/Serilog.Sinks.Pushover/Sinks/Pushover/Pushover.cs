using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.Pushover.Sinks.Pushover
{
    public class Pushover : ILogEventSink
    {
        private readonly ITextFormatter _messageFormatter;
        private readonly ITextFormatter _titleFormatter;
        private readonly string _apiUri;
        private readonly string _token;
        private readonly string _userOrGroupKey;
        private readonly string[] _devices;
        private readonly PushoverMessagePriority _pushoverMessagePriority;
        private readonly LogEventLevel _logEventLevel;

        public Pushover(ITextFormatter titleFormatter,
            ITextFormatter messageFormatter,
            string apiUri,
            string token,
            string userOrGroupKey,
            string[] devices,
            PushoverMessagePriority pushoverMessagePriority,
            LogEventLevel logEventLevel)
        {
            _titleFormatter = titleFormatter;
            _messageFormatter = messageFormatter;
            _apiUri = apiUri;
            _token = token;
            _userOrGroupKey = userOrGroupKey;
            _devices = devices;
            _logEventLevel = logEventLevel;
            _pushoverMessagePriority = pushoverMessagePriority;
        }

        /// <summary>
        /// Emit the provided log event to the sink.
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Level < _logEventLevel)
                return;

            using (var titleBuffer = new StringWriter())
            using (var messageBuffer = new StringWriter())
            {
                _titleFormatter.Format(logEvent, titleBuffer);
                _messageFormatter.Format(logEvent, messageBuffer);

                var parameters = new NameValueCollection {
                    { "token", _token },
                    { "user", _userOrGroupKey },
                    { "title", titleBuffer.ToString() },
                    { "message", messageBuffer.ToString() },
                    { "device", string.Join(",", _devices ?? new[]{""}) },
                    { "priority", _pushoverMessagePriority.ToString("D") }
                };

                using (var client = new WebClient())
                {
                    try
                    {
                        client.UploadValues(new Uri(_apiUri), parameters);
                    }
                    catch (WebException webException)
                    {
                        var resp = new StreamReader(webException.Response.GetResponseStream()).ReadToEnd();
                        throw new HttpRequestException(resp, webException);
                    }
                }
            }
        }
    }
}
