using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace Game.Signals
{
    public class SignalBus
    {
        private Dictionary<Type, List<Delegate>> _signalCallbacks = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> callback, int priority = 0)
        {
            Type key = typeof(T);

            if (_signalCallbacks.ContainsKey(key))
            {
                _signalCallbacks[key].Add(callback);
                return;
            }

            _signalCallbacks.Add(key, new List<Delegate>() { callback });
        }

        public void Invoke<T>(T signal)
        {
            Type key = typeof(T);
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
            Type key = typeof(T);
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
