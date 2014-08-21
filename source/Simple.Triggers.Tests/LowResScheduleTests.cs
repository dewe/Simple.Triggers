using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;

namespace Simple.Triggers.Tests
{
    [TestFixture]
    public class LowResScheduleTests
    {
        private LowResSchedule _schedule;
        
        [SetUp]
        public void SetupWithMockInternals()
        {
            var internalCache = A.Fake<ObjectCache>();

            A.CallTo(() =>
                internalCache.Add(A<string>._, A<object>._, A<CacheItemPolicy>._, A<string>._))
                .Invokes((string key, object item, CacheItemPolicy policy, string region) => Task.Run(() =>
                    policy.RemovedCallback(null)));

            _schedule = new LowResSchedule(internalCache);
        }

        [Test]
        public void It_should_handle_null_action()
        {
            _schedule.Every(TimeSpan.FromMilliseconds(1));
        }

        [Test]
        public void It_should_trigger_within_20_sec()
        {
            bool wasCalled = false;
            var realSchedule = new LowResSchedule();

            realSchedule.Every(2.Seconds()).Action(() => wasCalled = true);

            // Internal timer frequency is 20s.
            Assert.That(() => wasCalled, Is.EqualTo(true).After(21000, 100));
        }

        [Test]
        public void It_should_continue_despite_action_fail()
        {
            int counter = 0;

            _schedule.Every(TimeSpan.FromMilliseconds(1)).Action(() =>
            {
                counter++;
                throw new Exception();
            });

            Assert.That(() => counter, Is.GreaterThan(2).After(100, 10));
        }
    }
}