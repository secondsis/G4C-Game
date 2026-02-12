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
        private bool promptEnabled = true;

        private void Awake()
        {
            interactionManager = GameObject.FindGameObjectWithTag("InteractionManager").GetComponent<InteractionManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!promptEnabled) return;
            Debug.Log("Interaction trigger entered");
            // Add to Interaction Manager
            if (other.CompareTag("Player"))
            {
                interactionManager.Interactions.Add(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!promptEnabled) return;
            Debug.Log("Interaction trigger exited");
            // Remove from InteractionManager
            if (other.CompareTag("Player"))
            {
                interactionManager.Interactions.Remove(this);
            }
        }

        public void PromptDisable()
        {
            promptEnabled = false;

            try
            {
                interactionManager.Interactions.Remove(this);
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