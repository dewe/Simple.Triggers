using System;
using NUnit.Framework;

namespace Simple.Triggers.Tests
{
    [TestFixture]
    public class LowResourceScheduleTests
    {
        [Test]
        public void It_should_continue_despite_action_fail()
        {
            var counter = 0;
            var schedule = new LowResourceSchedule();

            schedule.Every(2.Seconds()).Action(() =>
            {
                counter++;
                throw new Exception();
            });

            // Internal timer frequency is 20s.
            Assert.That(() => counter, Is.EqualTo(1).After(21000, 100));
        }
    }
}