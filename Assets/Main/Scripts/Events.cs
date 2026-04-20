using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts
{
    public static class Events
    {
        public static event Action<GameObject> OnToolEquip;
        public static event Action OnToolUnequip;
        public static event Action OnToolUse;

        public static event Action OnWaterWellInteract;
        
        // After exhausting ALL events, witness ending cutscene
        // {Name of Event}, {Corresponding Joe Talk}
        public static Dictionary<String, String> GameEvents = new()
        {
            { "Kevin", "Kevin?,Kevin,Kevin" }, // KevinsComeback -> JoeWhosKevin
            { "Factory", "Factory?,GotFactory,Factory" }, // FactoryInteract -> JoeHatesPollution
            { "Trucks", "Trucks?,GotTrucks,Trucks" }, // Truckfire -> JoeWhatTrucks
            { "Church", "Church?,GotChurch,Church" }, // ChurchInteract -> JoeChurch
            { "Farm", "Farm?,GotFarm,Farm" }, // Obtained when you finish Wise Joe's quests
            { "DirtHoles", "Dirt?,GotDirtHoles,DirtHoles" }, // DirtHoles -> JoeWhyDirt
            { "Pigs", "Pigs?,GotPigs,Pigs" }, // PigInteract -> JoeWhyPigs
            { "Chairs", "Chairs?,GotChairs,Chairs" } // ChairGod -> JoeWhatTheChair
        };
        
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