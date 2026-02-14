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
            if(!promptEnabled) return;
            if (!other.CompareTag("Player")) return;
            
            if (interactionType == 1 && !interactionManager.Interactions1.Contains(this))
            {
                interactionManager.Interactions1.Add(this);
            } 
            else if (!interactionManager.Interactions2.Contains(this))
            {
                DebugScript.BetterDebug("Adding Interaction2");
                interactionManager.Interactions2.Add(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!promptEnabled) return;
            Debug.Log("Interaction trigger entered");
            // Add to Interaction Manager
            if (other.CompareTag("Player"))
            {
                if (interactionType == 1)
                {
                    interactionManager.Interactions1.Add(this);
                }
                else
                {
                    DebugScript.BetterDebug("Enter: Adding Interaction2");
                    interactionManager.Interactions2.Add(this);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!promptEnabled) return;
            Debug.Log("Interaction trigger exited");
            // Remove from InteractionManager
            if (other.CompareTag("Player"))
            {
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