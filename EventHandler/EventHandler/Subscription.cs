using System;
using EventHandler.Interfaces;

namespace EventHandler
{
    public class Subscription<TEvent> : ISubscription where TEvent : IEvent
    {
        private readonly Action<IEvent> _envelopedAction;
        public bool IsExecutable { get; set; } = true;
        public int Priority { get; set; }
        public Delegate SubscriptionToken { get; }


        public Subscription(Action<TEvent> action)
        {
            _envelopedAction = eventBase => action((TEvent) eventBase);
            SubscriptionToken = action;
        }

        public Subscription(Action<TEvent> action, int priority)
        {
            _envelopedAction = eventBase => action((TEvent) eventBase);
            SubscriptionToken = action;
            Priority = priority;
        }

        public Subscription(Action<IEvent> action)
        {
            _envelopedAction = action;
            SubscriptionToken = action;
        }
        
        public Subscription(Action<IEvent> action, int priority)
        {
            _envelopedAction = action;
            SubscriptionToken = action;
            Priority = priority;
        }

        public void Execute(IEvent eventBase)
        {
            if (!IsExecutable) return;
            
            _envelopedAction.Invoke(eventBase);
        }
    }
}