using GreenPipes;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Hosts;
using MassTransit.Logging;
using MassTransit.Util;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;

namespace Miles.MassTransit.Unity
{
    public class UnityCompensateActivityFactory<TActivity, TLog> : CompensateActivityFactory<TActivity, TLog>
        where TActivity : class, CompensateActivity<TLog>
        where TLog : class
    {
        private static readonly ILog log = Logger.Get<UnityCompensateActivityFactory<TActivity, TLog>>();

        private readonly IUnityContainer container;

        public UnityCompensateActivityFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public async Task<ResultContext<CompensationResult>> Compensate(CompensateContext<TLog> context, IRequestPipe<CompensateActivityContext<TActivity, TLog>, CompensationResult> next)
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
                    log.DebugFormat("CompensateActivityFactory: Executing: {0}", TypeMetadataCache<TActivity>.ShortName);

                var activityContext = new HostCompensateActivityContext<TActivity, TLog>(activity, context);

                return await next.Send(activityContext).ConfigureAwait(false);
            }
        }
    }
}
