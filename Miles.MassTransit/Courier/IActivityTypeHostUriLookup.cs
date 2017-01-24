using System;

namespace Miles.MassTransit.Courier
{
    public interface IActivityTypeHostUriLookup
    {
        Uri Lookup<TArguments>() where TArguments : class;
    }
}