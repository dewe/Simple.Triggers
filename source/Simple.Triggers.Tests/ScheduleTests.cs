using System;
using NUnit.Framework;

namespace Simple.Triggers.Tests
{
    [TestFixture]
    public class ScheduleTests
    {
        private Schedule _schedule;
        private int _counter;

        [SetUp]
        public void Setup()
        {
            _schedule = new Schedule();
            _counter = 0;
        }

        [Test]
        public void It_should_wait_set_amount_of_time()
        {
            _schedule
                .Every(TimeSpan.FromMilliseconds(100))
                .Action(() => _counter++);

            Assert.That(() => _counter, Is.EqualTo(0).After(50, 50));
            Assert.That(() => _counter, Is.EqualTo(1).After(150, 50));
        }

        [Test]
        public void It_should_trigger_repeatedly()
        {
            _schedule
                .Every(TimeSpan.FromMilliseconds(100))
                .Action(() => _counter++);

            Assert.That(() => _counter, Is.GreaterThan(2).After(500, 50));
        }

        [Test]
        public void It_should_continue_despite_action_fail()
        {
            _schedule
                .Every(TimeSpan.FromMilliseconds(100))
                .Action(() =>
                {
                    _counter++;
                    throw new Exception();
                });

            Assert.That(() => _counter, Is.GreaterThan(2).After(500, 50));
        }

        [TearDown]
        public void StopSchedule()
        {
            _schedule.Dispose();
        }
    }
}