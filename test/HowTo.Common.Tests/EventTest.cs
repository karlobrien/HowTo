using System;
using Xunit;
using HowTo.Common;

namespace HowTo.Common.Tests
{
    public class EventTest
    {
        private bool _myEventRaised;

        [Fact]
        public void Validate_EventHasBeenRaised()
        {
            _myEventRaised = false;
            SampleForEvent se = new SampleForEvent();
            se.OnEvent += new MyEventHandler(MaxReached);

            se.AddToNumber(2);
            Assert.False(_myEventRaised);
            se.AddToNumber(9);
            Assert.False(_myEventRaised);
            se.AddToNumber(2);
            Assert.True(_myEventRaised);
        }

        private void MaxReached(object obj, MyEventArgs e)
        {
            _myEventRaised = true;
        }
    }
}
