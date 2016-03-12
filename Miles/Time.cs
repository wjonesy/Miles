using System;

namespace Miles
{
    public class Time : ITime
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}
