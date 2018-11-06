
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
    }
}