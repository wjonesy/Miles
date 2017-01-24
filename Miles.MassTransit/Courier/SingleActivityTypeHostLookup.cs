using System;

namespace Miles.MassTransit.Courier
{
    public class SingleActivityTypeHostLookup : IActivityTypeHostUriLookup
    {
        private readonly Uri hostAddress;

        public SingleActivityTypeHostLookup(Uri hostAddress)
        {
            this.hostAddress = hostAddress;
        }

        public Uri Lookup<TArguments>() where TArguments : class
        {
            return hostAddress;
        }
    }
}
