using MassTransit.Courier;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Courier
{
    public static class ItineraryBuilderExtensions
    {
        public static void AddActivity<TArguments>(this ItineraryBuilder builder, string name, Uri hostAddress)
        {
            builder.AddActivity(name, new Uri(hostAddress, typeof(TArguments).GenerateExecutionQueueName()));
        }

        public static void AddActivity<TArguments>(this ItineraryBuilder builder, string name, Uri hostAddress, object arguments)
        {
            builder.AddActivity(name, new Uri(hostAddress, typeof(TArguments).GenerateExecutionQueueName()), arguments);
        }

        public static void AddActivity<TArguments>(this ItineraryBuilder builder, string name, Uri hostAddress, IDictionary<string, object> arguments)
        {
            builder.AddActivity(name, new Uri(hostAddress, typeof(TArguments).GenerateExecutionQueueName()), arguments);
        }

        public static void AddActivity<TArguments>(this ItineraryBuilder builder, string name, Uri hostAddress, TArguments arguments)
        {
            builder.AddActivity(name, new Uri(hostAddress, typeof(TArguments).GenerateExecutionQueueName()), arguments);
        }
    }
}
