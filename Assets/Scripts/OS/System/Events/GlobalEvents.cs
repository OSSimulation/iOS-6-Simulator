using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OS6.Events
{
    public class GlobalEvents : MonoBehaviour
    {
        private static readonly Dictionary<string, Action> eventList = new();

        public static void Subscribe(string eventName, Action action)
        {
            if (!eventList.ContainsKey(eventName))
                eventList[eventName] = action;
            else
                eventList[eventName] += action;
        }

        public static void Unsubscribe(string eventName, Action action)
        {
            if (!eventList.ContainsKey(eventName)) return;

            eventList[eventName] -= action;

            if (eventList[eventName] == null)
                eventList.Remove(eventName);
        }

        public static void PublishEventMessage(string eventName)
        {
            if (eventList.TryGetValue(eventName, out Action action))
                action?.Invoke();
        }
    }
}