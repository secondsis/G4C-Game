using System;
using UnityEngine;

namespace Main.Scripts
{
    public static class ToolEvents
    {
        public static event Action<GameObject> OnToolEquip;
        
        public static void InvokeToolEquip(GameObject tool)
        {
            OnToolEquip?.Invoke(tool);
        }
    }
}