using GreenPipes;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Hosts;
using MassTransit.Logging;
using MassTransit.Util;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;

namespace Miles.MassTransit.Unity.Courier
{
    public class MilesUnityExecuteActivityFactory<TActivity, TArguments> : ExecuteActivityFactory<TActivity, TArguments>
        where TActivity : class, ExecuteActivity<TArguments>
        where TArguments : class
    {
        private static readonly ILog log = Logger.Get<MilesUnityExecuteActivityFactory<TActivity, TArguments>>();

        private readonly IUnityContainer container;

        public MilesUnityExecuteActivityFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public async Task<ResultContext<ExecutionResult>> Execute(ExecuteContext<TArguments> context, IRequestPipe<ExecuteActivityContext<TActivity, TArguments>, ExecutionResult> next)
        {
            using (var childContainer = container.CreateChildContainer())
            {
                childContainer.RegisterInstance<ConsumeContext>(context.ConsumeContext)
                    .RegisterInstance<IPublishEndpoint>(context)
                    .RegisterInstance<ISendEndpointProvider>(context);

                var activity = childContainer.Resolve<TActivity>();
                if (activity == null)
                    throw new Exception($"Unable to resolve consumer type '{typeof(TActivity).FullName}'.");    // TODO: Better exception

                if (log.IsDebugEnabled)
                    log.DebugFormat("ExecuteActivityFactory: Executing: {0}", TypeMetadataCache<TActivity>.ShortName);

                var activityContext = new HostExecuteActivityContext<TActivity, TArguments>(activity, context);

                return await next.Send(activityContext).ConfigureAwait(false);
            }
        }
    }
}
