using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace Game.Signals
{
    public class SignalBus
    {
        private Dictionary<string, List<object>> _signalCallbacks = new Dictionary<string, List<object>>();

        public void Subscribe<T>(Action<T> callback, int priority = 0)
        {
            string key = typeof(T).Name;

            if (_signalCallbacks.ContainsKey(key))
            {
                _signalCallbacks[key].Add(callback);
                return;
            }

            _signalCallbacks.Add(key, new List<object>() { callback });
        }

        public void Invoke<T>(T signal)
        {
            string key = typeof(T).Name;
            if (_signalCallbacks.ContainsKey(key))
            {
                foreach (var obj in _signalCallbacks[key])
                {
                    var callback = obj as Action<T>;
                    callback?.Invoke(signal);
                }
            }
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            string key = typeof(T).Name;
            if (_signalCallbacks.ContainsKey(key))
            {
                var callbackToDelete = _signalCallbacks[key].FirstOrDefault(x => x.Equals(callback));
                if (callbackToDelete != null)
                {
                    _signalCallbacks[key].Remove(callbackToDelete);
                }
                return;
            }

            Debug.LogErrorFormat("Trying to unsubscribe for not existing key! {0} ", key);
        }
    }
}
