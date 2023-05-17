using System;
using System.Collections.Generic;
using System.Linq;
using EventHandler.Interfaces;

namespace EventHandler
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<ISubscription>> _subscriptionsMap =
            new Dictionary<Type, List<ISubscription>>();

        private readonly Dictionary<Delegate, List<Type>> _typeMap = new Dictionary<Delegate, List<Type>>();

        public void Subscribe<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            GenericSubscribe(action, list =>  
            {
                list.Add(new Subscription<TEvent>(action));
                list.Sort((x, y) => -x.Priority.CompareTo(y.Priority));
            });
        }

        public void Subscribe<TEvent>(Action<TEvent> action, int priority) where TEvent : IEvent
        {
            GenericSubscribe(action, list =>  
            {
                list.Add(new Subscription<TEvent>(action, priority));
                list.Sort((x, y) => -x.Priority.CompareTo(y.Priority));
            });
        }

        private void GenericSubscribe<TEvent>(Action<TEvent> action, Action<List<ISubscription>> subscribeAction) where TEvent : IEvent
        {
            if (_subscriptionsMap.TryGetValue(typeof(TEvent), out var subscriptions))
            {
                subscribeAction(subscriptions);
            }
            else
            {
                _subscriptionsMap.Add(typeof(TEvent), new List<ISubscription>());
                subscribeAction(_subscriptionsMap[typeof(TEvent)]);
            }
            
            if (_typeMap.TryGetValue(action, out var types))
            {
                types.Add(typeof(TEvent));
            }
            else
            {
                _typeMap.Add(action, new List<Type>());
                _typeMap[action].Add(typeof(TEvent));
            }
        }
        
        public void Subscribe(Type type, Action<IEvent> action)
        {
            if (!type.GetInterfaces().Contains(typeof(IEvent)))
            {
                throw new ArgumentException($"{type} should implement IEventBase");
            }
            
            if (_subscriptionsMap.TryGetValue(type, out var subscriptions))
            {
                subscriptions.Add(new Subscription<IEvent>(action));
            }
            else
            {
                _subscriptionsMap.Add(type, new List<ISubscription>());
                _subscriptionsMap[type].Add(new Subscription<IEvent>(action));
            }
            
            if (_typeMap.TryGetValue(action, out var types))
            {
                types.Add(type);
            }
            else
            {
                _typeMap.Add(action, new List<Type>());
                _typeMap[action].Add(type);
            }
        }
        
        public void Subscribe<TValue>(Type type, Action<IValueEvent<TValue>> action)
        {
            if (!type.GetInterfaces().Contains(typeof(IEvent)))
            {
                throw new ArgumentException($"{type} should implement IEventBase");
            }
            
            if (_subscriptionsMap.TryGetValue(type, out var subscriptions))
            {
                subscriptions.Add(new Subscription<IValueEvent<TValue>>(action));
            }
            else
            {
                _subscriptionsMap.Add(type, new List<ISubscription>());
                _subscriptionsMap[type].Add(new Subscription<IValueEvent<TValue>>(action));
            }
            
            if (_typeMap.TryGetValue(action, out var types))
            {
                types.Add(type);
            }
            else
            {
                _typeMap.Add(action, new List<Type>());
                _typeMap[action].Add(type);
            }
        }

        public void Unsubscribe<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            
            var types = _typeMap[action];
            if (!types.Contains(typeof(TEvent))) return;
            
            ISubscription subscriptionToRemove = null;
            
            foreach (var subscription in _subscriptionsMap[typeof(TEvent)]
                .Where(subscription => subscription.SubscriptionToken == (Delegate) action))
            {
                subscriptionToRemove = subscription;
            }

            if (subscriptionToRemove == null) return;
            
            _subscriptionsMap[typeof(TEvent)].Remove(subscriptionToRemove);
            _typeMap[action].Remove(typeof(TEvent));
            if (_typeMap[action].Count == 0) _typeMap.Remove(action);
        }
        
        public void Unsubscribe(Type type, Action<IEvent> action)
        {
            if (!type.GetInterfaces().Contains(typeof(IEvent)))
            {
                throw new ArgumentException($"{type} should implement IEventBase");
            }
            
            if (action == null) throw new ArgumentNullException(nameof(action));
            
            var types = _typeMap[action];
            if (!types.Contains(type)) return;
            
            ISubscription subscriptionToRemove = null;
            
            foreach (var subscription in _subscriptionsMap[type]
                .Where(subscription => subscription.SubscriptionToken == (Delegate) action))
            {
                subscriptionToRemove = subscription;
            }

            if (subscriptionToRemove == null) return;
            
            _subscriptionsMap[type].Remove(subscriptionToRemove);
            _typeMap[action].Remove(type);
            if (_typeMap[action].Count == 0) _typeMap.Remove(action);
        }

        public void Publish(IEvent eventBase)
        {
            if (!_subscriptionsMap.TryGetValue(eventBase.GetType(), out var subscriptions)) return;
            foreach (var subscription in subscriptions)
            {
                subscription.Execute(eventBase);
            }
        }

        public void SetEnableExecuteActions(Type type, bool isExecutable)
        {
            foreach (var subscription in _subscriptionsMap[type])
            {
                subscription.IsExecutable = isExecutable;
            }
        }
    }
}
