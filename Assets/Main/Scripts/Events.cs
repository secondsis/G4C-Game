using System;
using UnityEngine;

namespace Main.Scripts
{
    public static class Events
    {
        public static event Action<GameObject> OnToolEquip;
        public static event Action OnToolUnequip;
        public static event Action OnToolUse;

        public static event Action OnWaterWellInteract;
        
        
        public static void InvokeToolEquip(GameObject tool)
        {
            OnToolEquip?.Invoke(tool);
        }

        public static void InvokeToolUnequip()
        {
            OnToolUnequip?.Invoke();
        }

        // Invoked when player clicks
        public static void InvokeToolUse()
        {
            if (InventoryManager.Instance.CurrentlyEquipped != null)
            {
                OnToolUse?.Invoke();
                // Individual tools may connect to this event using a script under the prefab
            }
        }

        public static void InvokeWaterWellInteract()
        {
            OnWaterWellInteract?.Invoke();
        }
    }
}