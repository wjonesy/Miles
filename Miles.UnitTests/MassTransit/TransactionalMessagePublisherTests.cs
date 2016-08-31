using FakeItEasy;
using Miles.MassTransit;
using Miles.Messaging;
using Miles.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.UnitTests.MassTransit
{
    [TestFixture]
    public class TransactionalMessagePublisherTests
    {
        [Test]
        public void Publish_DoesNotSaveMessages_WhenTransactionsAreNotCommitted()
        {
            // Arrange
            var eventType = new EventType();
            var commandType = new CommandType();

            var fakeTransactionContext = CreateTransactionContext();
            var fakeOutgoingRepo = A.Fake<IOutgoingMessageRepository>();
            var fakeActivityContext = CreateActivityContext();

            var publisher = new TransactionalMessagePublisher(fakeTransactionContext, fakeOutgoingRepo, new Time(), fakeActivityContext, null, null);

            // Act
            ((IEventPublisher)publisher).Publish(eventType);
            ((ICommandPublisher)publisher).Publish(commandType);

            // Assert
            A.CallTo(() => fakeOutgoingRepo.SaveAsync(A<IEnumerable<OutgoingMessage>>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public async Task Publish_SaveMessages_WhenTransactionsAreCommitted()
        {
            // Arrange
            var eventType = new EventType();
            var commandType = new CommandType();

            var fakeTransactionContext = CreateTransactionContext();
            var fakeOutgoingRepo = A.Fake<IOutgoingMessageRepository>();
            var fakeActivityContext = CreateActivityContext();

            var publisher = new TransactionalMessagePublisher(fakeTransactionContext, fakeOutgoingRepo, new Time(), fakeActivityContext, null, null);

            // Act
            using (var transaction = await fakeTransactionContext.BeginAsync())
            {
                ((IEventPublisher)publisher).Publish(eventType);
                ((ICommandPublisher)publisher).Publish(commandType);

                await transaction.CommitAsync();
            }

            // Assert
            A.CallTo(() => fakeOutgoingRepo.SaveAsync(A<IEnumerable<OutgoingMessage>>.Ignored)).MustHaveHappened();
        }

        [Test]
        public async Task Publish_MessagesDispatched_WhenTransactionsAreCommitted()
        {
            // Arrange
            var eventType = new EventType();
            var commandType = new CommandType();

            var fakeTransactionContext = CreateTransactionContext();
            var fakeOutgoingRepo = A.Fake<IOutgoingMessageRepository>();
            var fakeActivityContext = CreateActivityContext();
            var fakeMessageDispatcher = A.Fake<IMessageDispatcher>();

            var publisher = new TransactionalMessagePublisher(fakeTransactionContext, fakeOutgoingRepo, new Time(), fakeActivityContext, fakeMessageDispatcher, null);

            // Act
            using (var transaction = await fakeTransactionContext.BeginAsync())
            {
                ((IEventPublisher)publisher).Publish(eventType);
                ((ICommandPublisher)publisher).Publish(commandType);

                await transaction.CommitAsync();
            }

            // Assert
            A.CallTo(() => fakeMessageDispatcher.DispatchAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustHaveHappened();
        }

        [Test]
        public async Task Publish_MessagesAreNotDispatched_WhenTransactionsCommitFails()
        {
            // Arrange
            var eventType = new EventType();
            var commandType = new CommandType();

            var fakeTransactionContext = CreateTransactionContext(failCommit: true);
            var fakeOutgoingRepo = A.Fake<IOutgoingMessageRepository>();
            var fakeActivityContext = CreateActivityContext();
            var fakeMessageDispatcher = A.Fake<IMessageDispatcher>();

            var publisher = new TransactionalMessagePublisher(fakeTransactionContext, fakeOutgoingRepo, new Time(), fakeActivityContext, fakeMessageDispatcher, null);

            // Act
            using (var transaction = await fakeTransactionContext.BeginAsync())
            {
                ((IEventPublisher)publisher).Publish(eventType);
                ((ICommandPublisher)publisher).Publish(commandType);

                await transaction.CommitAsync();
            }

            // Assert
            A.CallTo(() => fakeOutgoingRepo.SaveAsync(A<IEnumerable<OutgoingMessage>>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakeMessageDispatcher.DispatchAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustNotHaveHappened();
        }

        [Test]
        public async Task Register_ProcessorsExecutedPriorToDatabaseCommit_WhenTransactionAreCommitted()
        {
            // Arrange
            var eventType = new EventType();
            var commandType = new CommandType();

            var fakeTransactionContext = CreateTransactionContext(failCommit: true);
            var fakeOutgoingRepo = A.Fake<IOutgoingMessageRepository>();
            var fakeActivityContext = CreateActivityContext();
            var fakeMessageDispatcher = A.Fake<IMessageDispatcher>();

            var fakeEventProcessor = A.Fake<IMessageProcessor<EventType>>();
            var fakeCommandProcessor = A.Fake<IMessageProcessor<CommandType>>();

            var publisher = new TransactionalMessagePublisher(fakeTransactionContext, fakeOutgoingRepo, new Time(), fakeActivityContext, fakeMessageDispatcher, null);

            // Act
            using (var transaction = await fakeTransactionContext.BeginAsync())
            {
                ((IEventPublisher)publisher).Register(fakeEventProcessor);
                ((ICommandPublisher)publisher).Register(fakeCommandProcessor);

                ((IEventPublisher)publisher).Publish(eventType);
                ((ICommandPublisher)publisher).Publish(commandType);

                await transaction.CommitAsync();
            }

            // Assert
            A.CallTo(() => fakeOutgoingRepo.SaveAsync(A<IEnumerable<OutgoingMessage>>.Ignored)).MustHaveHappened().Then(
                A.CallTo(() => fakeEventProcessor.ProcessAsync(A<EventType>.Ignored)).MustHaveHappened()).Then(
                A.CallTo(() => fakeMessageDispatcher.DispatchAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustHaveHappened());

            A.CallTo(() => fakeOutgoingRepo.SaveAsync(A<IEnumerable<OutgoingMessage>>.Ignored)).MustHaveHappened().Then(
                A.CallTo(() => fakeCommandProcessor.ProcessAsync(A<CommandType>.Ignored)).MustHaveHappened()).Then(
                A.CallTo(() => fakeMessageDispatcher.DispatchAsync(A<IEnumerable<OutgoingMessageForDispatch>>.Ignored)).MustHaveHappened());
        }

        private ITransactionContext CreateTransactionContext(bool failCommit = false)
        {
            var fakeTransaction = A.Fake<ITransaction>();
            var fakeTransactionContext = A.Fake<ITransactionContext>();
            var preCommitHook = new Hook<object, EventArgs>();
            var postCommitHook = new Hook<object, EventArgs>();

            A.CallTo(() => fakeTransaction.CommitAsync()).Invokes(async () =>
            {
                await preCommitHook.ExecuteAsync(fakeTransactionContext, new EventArgs());

                if (failCommit)
                    throw new Exception();

                await postCommitHook.ExecuteAsync(fakeTransactionContext, new EventArgs());
            }).Returns(Task.FromResult(0));

            A.CallTo(() => fakeTransactionContext.BeginAsync()).Returns(Task.FromResult(fakeTransaction));
            A.CallTo(() => fakeTransactionContext.PreCommit).Returns(preCommitHook);
            A.CallTo(() => fakeTransactionContext.PostCommit).Returns(postCommitHook);
            return fakeTransactionContext;
        }

        private IActivityContext CreateActivityContext()
        {
            var fakeActivityContext = A.Fake<IActivityContext>();
            A.CallTo(() => fakeActivityContext.CorrelationId).Returns(Guid.NewGuid());
            return fakeActivityContext;
        }
    }

    public class EventType
    { }

    public class CommandType
    { }
}
