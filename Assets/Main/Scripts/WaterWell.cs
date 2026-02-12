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
        if (!obj.name.Equals("WateringCan")) return;
        _interaction.PromptEnable();
    }

    private void CheckDisableInteraction()
    {
        // Need to check if obj name is watering can but can't.?
        if (InventoryManager.Instance.CurrentlyEquipped != null &&
            InventoryManager.Instance.CurrentlyEquipped.Item.ItemName.Equals("Watering Can"))
        {
            DebugScript.BetterDebug("Disabled watering can");
            _interaction.PromptDisable();
        }
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
