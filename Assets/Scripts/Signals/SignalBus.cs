using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Signals
{
    public class SignalBus
    {
        private readonly Dictionary<Type, object> _signalCallbacks = new(16);

        public void Subscribe<T>(Action<T> callback)
        {
            if (callback == null) return;

            var key = typeof(T);

            if (!_signalCallbacks.TryGetValue(key, out var obj))
            {
                var list = new List<Action<T>>(4);
                list.Add(callback);
                _signalCallbacks.Add(key, list);
                return;
            }

            ((List<Action<T>>)obj).Add(callback);
        }

        public void Invoke<T>(T signal)
        {
            var key = typeof(T);

            if (!_signalCallbacks.TryGetValue(key, out var obj))
                return;

            var list = (List<Action<T>>)obj;

            for (int i = 0; i < list.Count; i++)
            {
                list[i](signal);
            }
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            if (callback == null) return;

            var key = typeof(T);

            if (!_signalCallbacks.TryGetValue(key, out var obj))
            {
                Debug.LogError($"Trying to unsubscribe for not existing key! {key}");
                return;
            }

            var list = (List<Action<T>>)obj;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == callback)
                {
                    int last = list.Count - 1;
                    list[i] = list[last];
                    list.RemoveAt(last);
                    break;
                }
            }

            if (list.Count == 0)
            {
                _signalCallbacks.Remove(key);
            }
        }
    }
}