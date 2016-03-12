using System;

namespace Miles.MassTransit
{
    public class OutgoingEvent
    {
        protected OutgoingEvent()
        { }

        public OutgoingEvent(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }

        public DateTime? EventDispatched { get; private set; }

        public void Dispatched(ITime time)
        {
            EventDispatched = time.Now;
        }
    }
}
