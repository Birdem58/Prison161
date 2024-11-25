using PuppetOfShadows.EventBinding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace prison161.EventBus
{

    public static class EventBus<T>
    {
        //Subscribe
        //To subscribe event.

        //UnSubscribe
        //To unsubscrive

        //Invoke
        //To emit events
        static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

        public static void Register(EventBinding<T> binding) => bindings.Add(binding);
        public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);

        public static void Raise(T @event)
        {
            foreach (var binding in bindings)
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }

        static void Clear()
        {
            Debug.Log($"Clearing {typeof(T).Name} bindings");
            bindings.Clear();
        }

    }
}

namespace PuppetOfShadows.EventBinding
{
    public interface IEventBinding<T>
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }

    public class EventBinding<T> : IEventBinding<T>
    {
        Action<T> onEvent = _ => { };
        Action onEventNoArgs = () => { };

        Action<T> IEventBinding<T>.OnEvent
        {
            get => onEvent;
            set => onEvent = value;
        }

        Action IEventBinding<T>.OnEventNoArgs
        {
            get => onEventNoArgs;
            set => onEventNoArgs = value;
        }

        public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;
        public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

        public void Add(Action onEvent) => onEventNoArgs += onEvent;
        public void Remove(Action onEvent) => onEventNoArgs -= onEvent;

        public void Add(Action<T> onEvent) => this.onEvent += onEvent;
        public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;
    }
}