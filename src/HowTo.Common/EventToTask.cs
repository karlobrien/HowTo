
using System;

namespace HowTo.Common
{
    public delegate void MyEventHandler(object source, MyEventArgs e);
    public class MyEventArgs : EventArgs
    {
        private string EventInfo;
        public MyEventArgs(string Text)
        {
            EventInfo = Text;
        }
        public string GetInfo()
        {
            return EventInfo;
        }
    }

    public class SampleForEvent
    {
        public event MyEventHandler OnEvent;
        public int Number { get; private set;}
        public SampleForEvent()
        {
            Number = 0;
        }

        public void AddToNumber(int adder)
        {
            if (Number > 10)
                OnEvent?.Invoke(this, new MyEventArgs("Too Big"));
            else
                Number += adder;
        }
    }
}