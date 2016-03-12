using System.Collections.Generic;

namespace Miles.MassTransit
{
    public interface IOutgoingEventRepository
    {
        void Save(IEnumerable<OutgoingEvent> evts);
        void Save(OutgoingEvent evt, bool ignoreTransaction = false);
    }
}
