using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Scripts
{
    public static class G4CInputManager
    {
        public static readonly PlayerInputActions G4CPlayerInput = new PlayerInputActions();

        private static Interactable _currentInteractable;
        private static Interactable _currentInteractable2;
        
        private static readonly List<Interactable> InteractionList = new List<Interactable>();
        private static readonly List<Interactable> InteractionList2 = new List<Interactable>();
        
        public static bool IsBlocked(InteractionType interactionType, Action onInteract)
        {
            if (_currentInteractable == null) return false;

            return !_currentInteractable.RawEquals(interactionType, onInteract);
        }

        public static void RegisterInteract(InteractionType interactionType, Action newOnInteract, int interactNum=1)
        {
            // if (currentInteractionType > interactionType) return;
            // // NEVER LET THERE BE TWO INTERACTIONS OF THE SAME TYPE
            switch (interactNum)
            {
                case 1: 
                {
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
                    break;
                }
                case 2:
                {
                    if (_currentInteractable2?.GetInteractType() == interactionType)
                    {
                        Debug.LogWarning("NEVER LET THERE BE TWO INTERACTIONS OF THE SAME TYPE");
                    }

                    Interactable newInteractable = new Interactable(interactionType, newOnInteract);

                    if (_currentInteractable2 == null || interactionType > _currentInteractable2.GetInteractType())
                    {
                        _currentInteractable2 = newInteractable;
                    }
                    InteractionList2.Add(newInteractable);
                    break;
                }
            }


        }

        public static void SetInteract(InteractionType interactionType, Action newOnInteract, bool enabled, int interactNum=1)
        {
            switch (interactNum)
            {
                case 1:
                {
                    InteractionList.Find(i => i.RawEquals(interactionType, newOnInteract)).TempDisabled = !enabled;
                    break;
                }
                case 2:
                {
                    InteractionList2.Find(i => i.RawEquals(interactionType, newOnInteract)).TempDisabled = !enabled;
                    break;
                }
            }
 
        }

        public static void RemoveInteract(InteractionType interactionType, Action oldOnInteract, int interactNum=1)
        {
            switch (interactNum)
            {
                case 1:
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

                    break;
                }
                case 2:
                {
                    Interactable oldInteractable = new Interactable(interactionType, oldOnInteract);
                    InteractionList2.Remove(oldInteractable);
                    if (oldInteractable.Equals(_currentInteractable2))
                    {
                        // Recalc the currentInteraction
                        Interactable bestInteractable = InteractionList2[0];
                        foreach (Interactable interactable in InteractionList2)
                        {
                            if (interactable.GetInteractType() > bestInteractable.GetInteractType())
                            {
                                bestInteractable = interactable;
                            }
                        }

                        _currentInteractable2 = bestInteractable;
                    }

                    break;
                }
            }

        }

        private static void OnInteract(InputAction.CallbackContext ctx)
        {
            DebugScript.BetterDebug("OnInteract was called! Current Interactable: " +  _currentInteractable?.GetInteractType());
            if (_currentInteractable != null && !_currentInteractable.TempDisabled)
            {
                // Why is _currentInteractable not existant check
                _currentInteractable.Interact();
            }
        }

        private static void OnInteract2(InputAction.CallbackContext ctx)
        {
            DebugScript.BetterDebug("OnInteract2: " + _currentInteractable2?.GetInteractType());
            
            if (_currentInteractable2 != null && !_currentInteractable2.TempDisabled)
            {
                // This is called when watering plot.
                DebugScript.BetterDebug("OnInteract2 called!");
                _currentInteractable2.Interact();
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
            G4CPlayerInput.Player.Interact2.performed += OnInteract2;
            G4CPlayerInput.Player.ToggleInventory.performed += OnToggleInventory;
            G4CPlayerInput.Player.Click.performed += OnClick;
            
        }
    }
}