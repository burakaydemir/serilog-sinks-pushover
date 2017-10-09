﻿using System;
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
                .WriteTo.PushoverSink(LogEventLevel.Information, token: token, userOrGroupKey: userOrGroupKey).CreateLogger())
            {
                pushoverLoger.Information("test {notification}", "notification");
            }

            Assert.Equal(Enumerable.Empty<string>(), _selfLogMessages);
        }

        [TestMethod]
        public void Can_Send_PushMessage_With_Exception()
        {
            using (var pushoverLoger = new LoggerConfiguration()
                .WriteTo.PushoverSink(LogEventLevel.Information, token: token, userOrGroupKey: userOrGroupKey).CreateLogger())
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
        public void Minimum_Level_Is_Exception()
        {
            using (var pushoverLoger = new LoggerConfiguration()
                .WriteTo.PushoverSink(LogEventLevel.Error, token: token, userOrGroupKey: userOrGroupKey).CreateLogger())
            {
                pushoverLoger.Information("The information message that never reach");
            }

            Assert.Equal(Enumerable.Empty<string>(), _selfLogMessages);
        }
    }
}
