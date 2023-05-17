using System;

namespace EventHandler.Interfaces
{
    public interface ISubscription
    {
        bool IsExecutable { get; set; }
        Delegate SubscriptionToken { get; }
        void Execute(IEvent eventBase);
        int Priority { get; set; }
    }
}