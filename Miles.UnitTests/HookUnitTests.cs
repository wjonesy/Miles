using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Miles.UnitTests
{
    [TestFixture]
    public class HookUnitTests
    {
        [Test]
        public void Register_RegisteredProcessor_NoProcessorsRegistered()
        {
            var hook = new Hook<object, EventArgs>();

            bool executed = false;
            hook.Register((s, e) =>
            {
                executed = true;
                return Task.FromResult(0);
            });
            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            Assert.That(executed, Is.True);
        }

        [Test]
        public void Register_RegisteredProcessors_NoProcessorsRegistered()
        {
            var hook = new Hook<object, EventArgs>();

            bool executedOne = false;
            hook.Register((s, e) =>
            {
                executedOne = true;
                return Task.FromResult(0);
            });
            bool executedTwo = false;
            hook.Register((s, e) =>
            {
                executedTwo = true;
                return Task.FromResult(0);
            });

            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            Assert.That(executedOne, Is.True);
            Assert.That(executedTwo, Is.True);
        }

        [Test]
        public void Register_DuplicateNotRegistered_IfAlreadyRegistered()
        {
            var hook = new Hook<object, EventArgs>();
            int calls = 0;
            var handler = new Func<object, EventArgs, Task>((s, e) =>
            {
                ++calls;
                return Task.FromResult(0);
            });

            hook.Register(handler);
            hook.Register(handler);

            hook.ExecuteAsync(new object(), new EventArgs()).Wait();

            Assert.That(calls, Is.EqualTo(1));
        }
    }
}
