using System;
using NUnit.Framework;

namespace Simple.Triggers.Tests
{
    [TestFixture]
    public class MultiScheduleTests
    {
        [Test]
        public void It_should_accept_multiple_schedules()
        {
            var counter1 = 0;
            var counter2 = 0;

            var schedule = new MultiSchedule();
            schedule.Every(TimeSpan.FromMilliseconds(100), () => counter1++);
            schedule.Every(TimeSpan.FromMilliseconds(150), () => counter2++);

            Assert.That(() => counter1, Is.GreaterThan(0).After(500, 50));
            Assert.That(() => counter2, Is.GreaterThan(0).After(500, 50));
        }
    }
}