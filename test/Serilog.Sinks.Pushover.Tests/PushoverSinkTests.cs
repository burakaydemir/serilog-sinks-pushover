using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Debugging;
using Serilog.Events;
using Assert = Xunit.Assert;

namespace Serilog.Sinks.Pushover.Tests
{
    [TestClass]
    public class PushoverSinkTests
    {
        private string token = "xxx";
        private string userOrGroupKey = "xxx";
        readonly List<string> _selfLogMessages = new List<string>();

        public PushoverSinkTests()
        {
            SelfLog.Enable(_selfLogMessages.Add);
        }

        [TestMethod]
        public void Can_Send_PushMessage()
        {
            using (var pushoverLoger = new LoggerConfiguration()
                .WriteTo.PushoverSink(token, userOrGroupKey, LogEventLevel.Information).CreateLogger())
            {
                pushoverLoger.Information("test {notification}", "notification");
            }

            Assert.Equal(Enumerable.Empty<string>(), _selfLogMessages);
        }

        [TestMethod]
        public void Can_Send_PushMessage_With_Exception()
        {
            using (var pushoverLoger = new LoggerConfiguration()
                .WriteTo.PushoverSink(token, userOrGroupKey, LogEventLevel.Information).CreateLogger())
            {
                try
                {
                    throw new DivideByZeroException();
                }
                catch (Exception e)
                {
                    pushoverLoger.Error(e, "something went wrong! {@UserInfo}", new { UserName = "b.a", Name = "Burak", Surname = "Aydemir" });
                }
            }

            Assert.Equal(Enumerable.Empty<string>(), _selfLogMessages);
        }

        [TestMethod]
        public void When_Minimum_Level_Is_Exception_Then_Dont_Send_Infromation()
        {
            using (var pushoverLoger = new LoggerConfiguration()
                .WriteTo.PushoverSink(token, userOrGroupKey, LogEventLevel.Error).CreateLogger())
            {
                pushoverLoger.Information("The information message that never reach");
            }

            Assert.Equal(Enumerable.Empty<string>(), _selfLogMessages);
        }

        [TestMethod]
        public void Can_Send_When_Minimum_Level_Same_as_LogEventLevel()
        {
            using (var pushoverLoger = new LoggerConfiguration()
                .WriteTo.PushoverSink(token, userOrGroupKey, LogEventLevel.Error).CreateLogger())
            {
                pushoverLoger.Error("This message must be reach");
            }

            Assert.Equal(Enumerable.Empty<string>(), _selfLogMessages);
        }

        [TestMethod]
        public void Should_push_message_text_changable()
        {
            using (var pushoverLoger = new LoggerConfiguration()
                .WriteTo.PushoverSink(token, userOrGroupKey, LogEventLevel.Error, 
                            outputTitleTemplate: "|{Level}| {Message}", 
                            outputMessageTemplate: "Below error throwed! {Exception}").CreateLogger())
            {
                try
                {
                    throw new DivideByZeroException();
                }
                catch (Exception e)
                {
                    pushoverLoger.Error(e, "something went wrong! {UserName}", "b.b");
                }
            }

            Assert.Equal(Enumerable.Empty<string>(), _selfLogMessages);
        }
    }
}