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
        public void It_should_trigger_action()
        {
            _schedule
                .Every(TimeSpan.FromMilliseconds(100))
                .Action(() => _counter++);

            Assert.That(() => _counter, Is.GreaterThan(2).After(500, 50));
        }

        [Test]
        public void It_should_wait_before_trigger_action()
        {
            _schedule
                .Every(TimeSpan.FromMilliseconds(100))
                .Action(() => _counter++);

            Assert.That(() => _counter, Is.EqualTo(0).After(50, 50));
            Assert.That(() => _counter, Is.EqualTo(1).After(150, 50));
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

        [Test]
        public void It_should_not_care_about_order_of_methods_called()
        {
            // call Action() before Every()
            _schedule
                .Action(() => _counter++)
                .Every(TimeSpan.FromMilliseconds(100));

            Assert.That(() => _counter, Is.GreaterThan(2).After(500, 50));
        }

        [Test]
        public void It_should_handle_null_action()
        {
            _schedule.Every(TimeSpan.FromMilliseconds(1));
        }

        [TearDown]
        public void StopSchedule()
        {
            _schedule.Dispose();
        }
    }
}