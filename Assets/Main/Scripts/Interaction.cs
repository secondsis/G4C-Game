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

        private void Awake()
        {
            interactionManager = GameObject.FindGameObjectWithTag("InteractionManager").GetComponent<InteractionManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Interaction trigger entered");
            // Add to Interaction Manager
            if (other.CompareTag("Player"))
            {
                interactionManager.Interactions.Add(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Interaction trigger exited");
            // Remove from InteractionManager
            if (other.CompareTag("Player"))
            {
                interactionManager.Interactions.Remove(this);
            }
        }
    }
}