using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShUtilities.Threading
{
    /// <summary>
    /// Represents an actor in the Actor Model that may have changes applied to it via callbacks posted
    /// to it. In <see cref="SynchronizationContext.OperationCompleted"/> the actor can then update its
    /// externally visible state at once based on the one or more updates made to it via the callbacks.
    /// </summary>
    public partial class ActorSynchronizationContext: SynchronizationContext, IActorOptions
    {
        private SynchronizationContext _previousSynchronizationContext;

        public Actor Actor { get; }
        public TaskScheduler Scheduler { get; }

        public ActorSynchronizationContext(TaskScheduler scheduler)
        {
            Scheduler = scheduler;
            Actor = new Actor(this);
        }

        public override void Post(SendOrPostCallback callback, object state) => Actor.Post(callback, state);

        public override void Send(SendOrPostCallback callback, object state) => SynchronizationContextExtensions.SendHelper(this, callback, state);

        public override void OperationStarted()
        {
            base.OperationStarted();
            _previousSynchronizationContext = Current;
            SetSynchronizationContext(this);
        }

        public override void OperationCompleted()
        {
            SetSynchronizationContext(_previousSynchronizationContext);
            base.OperationCompleted();
        }

        public void ScheduleAsyncExecution(Action action)
        {
            new Task(action).Start(Scheduler);
        }
    }
}
