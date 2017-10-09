using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Events;

namespace Serilog.Sinks.Pushover.Tests
{
    [TestClass]
    public class PushoverSinkTests
    {
        private string token = "xxx";
        private string userOrGroupKey = "xxx";


        [TestMethod]
        public void Can_Send_PushMessage()
        {
            using (var pushoverLoger = new LoggerConfiguration()
                .WriteTo.PushoverSink(LogEventLevel.Information, token: token, userOrGroupKey: userOrGroupKey).CreateLogger())
            {
                pushoverLoger.Information("test {notification}", "notification");
            }
        }

        [TestMethod]
        public void Can_Send_PushMessage_With_Exception()
        {
            using (var pushoverLoger = new LoggerConfiguration()
                .WriteTo.PushoverSink(LogEventLevel.Information, token: token, userOrGroupKey: userOrGroupKey).CreateLogger())
            {
                try
                {
                    var zero = 0;
                    var value = 1;
                    var exceptionalProcess = value / zero;
                }
                catch (Exception e)
                {
                    pushoverLoger.Error(e, "something went wrong {@UserInfo}", new { UserName = "b.a", Name = "Burak", Surname = "Aydemir" });
                }
            }
        }

        [TestMethod]
        public void Minimum_Level_Is_Exception()
        {
            using (var pushoverLoger = new LoggerConfiguration()
                .WriteTo.PushoverSink(LogEventLevel.Error, token: token, userOrGroupKey: userOrGroupKey).CreateLogger())
            {
                pushoverLoger.Information("The information message that never reach");
            }
        }
    }
}
