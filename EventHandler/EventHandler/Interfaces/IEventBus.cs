using System;

namespace EventHandler.Interfaces
{
    public interface IEventBus
    {
        /// <summary>
        /// Subscribe the action to a specified event 
        /// </summary>
        /// <param name="action">The action which want to be invoked when the event is published</param>
        /// <typeparam name="TEvent">The type of an event which is derived from <see cref="IEvent"/></typeparam>
        void Subscribe<TEvent>(Action<TEvent> action) where TEvent : IEvent;
        /// <summary>
        /// Subscribe the action to a specified event 
        /// </summary>
        /// <param name="action">The action which want to be invoked when the event is published</param>
        /// /// <param name="priority">The priority of the execution order</param>
        /// <typeparam name="TEvent">The type of an event which is derived from <see cref="IEvent"/></typeparam>
        public void Subscribe<TEvent>(Action<TEvent> action, int priority) where TEvent : IEvent;
        /// <summary>
        /// Subscribe the action in run time to a specified event
        /// </summary>
        /// <param name="type">The type of an event which is derived from <see cref="IEvent"/></param>
        /// <param name="action">The action which want to be invoked when the event is published</param>
        /// <exception cref="ArgumentException">Throw if the type is not derived from IEventBase</exception>
        void Subscribe(Type type, Action<IEvent> action);
        /// <summary>
        /// Subscribe the action to a specified event 
        /// </summary>
        /// <param name="type">The type of an event which is derived from <see cref="IValueEvent"/></param>
        /// <param name="action">The action which want to remove from a specified event</param>
        void Subscribe<TValue>(Type type, Action<IValueEvent<TValue>> action);
        /// <summary>
        /// Unsubscribe the action to a specified event 
        /// </summary>
        /// <param name="action">The action which want to remove from a specified event</param>
        /// <typeparam name="TEvent">The type of an event which is derived from <see cref="IEvent"/></typeparam>
        void Unsubscribe<TEvent>(Action<TEvent> action) where TEvent : IEvent;
        /// <summary>
        /// Unsubscribe the action in run time to a specified event
        /// </summary>
        /// <param name="type">The type of an event which is derived from <see cref="IEvent"/></param>
        /// <param name="action">The action which want to remove from a specified event</param>
        /// <exception cref="ArgumentException">Throw if the type is not derived from IEventBase</exception>
        /// <exception cref="ArgumentNullException">Throw if the action is null</exception>
        void Unsubscribe(Type type, Action<IEvent> action);
        /// <summary>
        /// Publish the event and invoke actions which are subscribed to this
        /// </summary>
        /// <param name="event">The Event to publish</param>
        void Publish(IEvent @event);
        /// <summary>
        /// Set if actions can be triggered when publish a event
        /// </summary>
        /// <param name="type">The type of an event which is derived from <see cref="IEvent"/></param>
        /// <param name="isExecutable">set subscriptions are executable or not</param>
        void SetEnableExecuteActions(Type type, bool isExecutable);
    }
}