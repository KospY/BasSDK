using System;
using System.Collections.Generic;
using UnityEngine;


namespace ThunderRoad
{
    public class EventMessage : MonoBehaviour
    {
        [Multiline]
        public string text;
        public int priority;
        public float duration = 5;


        public void DebugLog(string text)
        {
            Debug.Log(text);
        }

        public void DebugLogObject(UnityEngine.Object obj)
        {
            Debug.Log("Hello " + obj.name);
        }

        public void ShowMessage()
        {
        }

        public void ShowMessage(string text)
        {
        }

        public void StopMessage()
        {
        }
    }
}
