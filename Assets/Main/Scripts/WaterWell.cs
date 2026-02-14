using System;
using Main.Scripts;
using UnityEngine;

public class WaterWell : MonoBehaviour
{
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

    // Upon interacting with the water well, invoke the watering can to refill
    public void OnInteractPrompt()
    {
        DebugScript.BetterDebug("OnInteractPrompt called");
        // Watering Can Refill
        Events.InvokeWaterWellInteract();
    }
}
