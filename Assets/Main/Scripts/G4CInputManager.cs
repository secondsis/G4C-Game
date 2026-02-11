using System;
using System.Collections.Generic;
using PrimeTweenDemo;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Scripts
{
    public static class G4CInputManager
    {
        public static readonly PlayerInputActions G4CPlayerInput = new PlayerInputActions();

        private static Interactable _currentInteractable;
        private static readonly List<Interactable> InteractionList = new List<Interactable>();
        
        public static bool IsBlocked(InteractionType interactionType, Action onInteract)
        {
            if (_currentInteractable == null) return false;
            // why does this always return false? 
            // There should be a case where the currentInteractable (dialogue) isnt equal to interaction1
            // Debug.Log("Current Interactable: " + _currentInteractable.GetInteractType() + " | Check: " + interactionType);
            // Why is it both interaction1? 
            return !_currentInteractable.RawEquals(interactionType, onInteract);
        }

        public static void RegisterInteract(InteractionType interactionType, Action newOnInteract)
        {
            // if (currentInteractionType > interactionType) return;
            // // NEVER LET THERE BE TWO INTERACTIONS OF THE SAME TYPE
            if (_currentInteractable?.GetInteractType() == interactionType)
            {
                Debug.LogWarning("NEVER LET THERE BE TWO INTERACTIONS OF THE SAME TYPE");
            }

            Interactable newInteractable = new Interactable(interactionType, newOnInteract);

            if (_currentInteractable == null || interactionType > _currentInteractable.GetInteractType())
            {
                _currentInteractable = newInteractable;
            }
            InteractionList.Add(newInteractable);
        }

        public static void SetInteract(InteractionType interactionType, Action newOnInteract, bool enabled)
        {
            InteractionList.Find(i => i.RawEquals(interactionType, newOnInteract)).TempDisabled = !enabled;
        }

        public static void RemoveInteract(InteractionType interactionType, Action oldOnInteract)
        {
            Interactable oldInteractable = new Interactable(interactionType, oldOnInteract);
            InteractionList.Remove(oldInteractable);
            if (oldInteractable.Equals(_currentInteractable))
            {
                // Recalc the currentInteraction
                Interactable bestInteractable = InteractionList[0];
                foreach (Interactable interactable in InteractionList)
                {
                    if (interactable.GetInteractType() > bestInteractable.GetInteractType())
                    {
                        bestInteractable = interactable;
                    }
                }

                _currentInteractable = bestInteractable;
            }
            
            // Debug.Log("New InteractionList: " + InteractionList);
        }
        
        private static void DefaultInteract()
        {
            Debug.Log("No Interaction Objects Nearby");
        }

        private static void OnInteract(InputAction.CallbackContext ctx)
        {
            DebugScript.BetterDebug("OnInteract was called! Current Interactable: " +  _currentInteractable?.GetInteractType());
            if (_currentInteractable != null && !_currentInteractable.TempDisabled)
            {
                _currentInteractable.Interact();
            }
        }

        private static void OnToggleInventory(InputAction.CallbackContext ctx)
        {
            InventoryManager.Instance.ToggleInventory(); 
        }
        
        private static void OnClick(InputAction.CallbackContext ctx)
        {
            Events.InvokeToolUse();
        }

        static G4CInputManager()
        {
            // Initialize each event
            G4CPlayerInput.Player.Interact.performed += OnInteract;
            G4CPlayerInput.Player.ToggleInventory.performed += OnToggleInventory;
            G4CPlayerInput.Player.Click.performed += OnClick;
            
        }
    }
}