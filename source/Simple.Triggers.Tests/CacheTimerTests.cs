using System;
using NUnit.Framework;

namespace Simple.Triggers.Tests
{
    [TestFixture]
    public class CacheTimerTests
    {
        public static int Counter = 0;
        private CacheTimer _timer;


        [Test]
        public void It_should_wait_set_amount_of_time()
        {
            _timer = new CacheTimer();

            _timer
                .Every(TimeSpan.FromSeconds(0.5))
                .Action(() => { Counter = 1; });

            Assert.That(() => Counter, Is.GreaterThan(0).After(2000, 100));
        }
    }
}