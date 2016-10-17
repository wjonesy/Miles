using MassTransit;
using MassTransit.ConsumeConfigurators;

namespace Miles.MassTransit.Configuration
{
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute
#pragma warning disable CS1658 // Warning is overriding an error
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <seealso cref="MassTransit.IPipeConfigurator{MassTransit.ConsumeContext{TMessage}}" />
    /// <seealso cref="MassTransit.ConsumeConfigurators.IConsumeConfigurator" />
    public interface IMessageProcessorMessageConfigurator<TMessage> : IPipeConfigurator<ConsumeContext<TMessage>>, IConsumeConfigurator
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
#pragma warning restore CS1658 // Warning is overriding an error
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute
        where TMessage : class
    { }
}