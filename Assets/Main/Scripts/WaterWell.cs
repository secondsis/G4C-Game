using System;
using Main.Scripts;
using UnityEngine;

public class WaterWell : MonoBehaviour
{
    // Subscribe to event to be known when player equips watering can
    // Then display the Interaction
    // Upon Interaction, tell the wateringcan to refill.

    private bool _isHoldingWateringCan;
    private Interaction _interaction;

    private void CheckEnableInteraction(GameObject obj)
    {
        DebugScript.BetterDebug(obj.name + " interact waterwell");
        
    }

    private void CheckDisableInteraction()
    {
        DebugScript.BetterDebug("Disabled tool");
    }

    private void Awake()
    {
        _interaction = GetComponent<Interaction>();
        _interaction.PromptDisable();

        Events.OnToolEquip += CheckEnableInteraction;
        Events.OnToolUnequip += CheckDisableInteraction;
    }

    public void OnInteractPrompt()
    {
        // Watering Can Refill
        Events.InvokeWaterWellInteract();
    }

    private void DisplayInteractPrompt()
    {
        _interaction.PromptEnable();
    }

    private void HideInteractPrompt()
    {
        _interaction.PromptDisable();
    }

    // Check if Player is holding Watering Can
    // private void OnTriggerStay(Collider other)
    // {
    //     if (!other.CompareTag("Player")) return;
    //     
    //     if (InventoryManager.Instance.CurrentlyEquipped != null && InventoryManager.Instance.CurrentlyEquipped.Item.ItemName.Equals("Watering Can"))
    //     {
    //         _interaction.PromptEnable();
    //     }
    //     else
    //     {
    //         if(_isHoldingWateringCan) HideInteractPrompt();
    //         _isHoldingWateringCan = false;
    //     }
    // }
}
