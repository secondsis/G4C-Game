using System;
using UnityEngine;

namespace Main.Scripts
{
    public static class Events
    {
        public static event Action<GameObject> OnToolEquip;
        public static event Action OnToolUnequip;
        
        public static void InvokeToolEquip(GameObject tool)
        {
            OnToolEquip?.Invoke(tool);
            // MAKE THE SEEDS USABLE
        }

        public static void InvokeToolUnequip()
        {
            OnToolUnequip?.Invoke();
        }
    }
}