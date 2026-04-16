using System;
using UnityEngine;
using UnityEngine.Events;

namespace Main.Scripts
{
    [RequireComponent(typeof(SphereCollider))]
    public class Interaction : MonoBehaviour
    {
        private InteractionManager interactionManager;
        public UnityEvent onInteraction;
        public string title = "Talk";
        public string interactionKeybind = "E";
        private bool promptEnabled = true;
        private int interactionType;

        private void Awake()
        {
            interactionManager = GameObject.FindGameObjectWithTag("InteractionManager").GetComponent<InteractionManager>();
            interactionType = interactionKeybind.Equals("E") ? 1 : 2;
        }

        private void OnTriggerStay(Collider other)
        {
            AddInteractsToList(other);
        }

        private void AddInteractsToList(Collider other)
        {
            if(!promptEnabled) return;
            if (!other.CompareTag("Player")) return;
            
            // Actively Runs if prompt is enabled and player exists in trigger
            
            // Interact1 means "E", or whatever it is binded to. This will add any Interact1's that aren't in the list yet
            if (interactionType == 1 && !interactionManager.Interactions1.Contains(this))
            {
                interactionManager.Interactions1.Add(this);
            } 
            // If it is Interact2, meaning "F", and it is not added yet, add it
            else if (!interactionManager.Interactions2.Contains(this))
            {
                DebugScript.BetterDebug("Adding Interaction2");
                interactionManager.Interactions2.Add(this);
            }
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (!promptEnabled) return;
        //     DebugScript.BetterDebug("Interaction trigger entered");
        //     // Add to Interaction Manager
        //     if (other.CompareTag("Player"))
        //     {
        //         if (interactionType == 1)
        //         {
        //             interactionManager.Interactions1.Add(this);
        //         }
        //         else
        //         {
        //             DebugScript.BetterDebug("Enter: Adding Interaction2");
        //             interactionManager.Interactions2.Add(this);
        //         }
        //     }
        // }

        private void OnTriggerExit(Collider other)
        {
            RemoveInteractsFromList(other);
        }

        private void RemoveInteractsFromList(Collider other)
        {
            if (!promptEnabled) return;
            if (!other.CompareTag("Player")) return;
            DebugScript.BetterDebug("Interaction trigger exited");
            
            // Remove from InteractionManager
            if (interactionType == 1)
            {
                interactionManager.Interactions1.Remove(this);
            }
            else
            {
                DebugScript.BetterDebug("Removing Interaction2");
                interactionManager.Interactions2.Remove(this);
            }
        }

        public void PromptDisable()
        {
            promptEnabled = false;

            try
            {
                if (interactionType == 1)
                {
                    interactionManager.Interactions1.Remove(this);
                }
                else
                {
                    interactionManager.Interactions2.Remove(this);
                }
                
            }
            catch (Exception e)
            {
                 DebugScript.BetterDebug("Interaction does not exist in list.");
            }
        }

        public void PromptEnable()
        {
            promptEnabled = true;
        }
    }
}