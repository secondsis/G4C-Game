using System;
using Main.Scripts;
using UnityEngine;

public class FertilizerPouch : MonoBehaviour
{

    private void OnFertilizerPouchUse()
    {
        DebugScript.BetterDebug("Fertilizer Pouch Use");
        // Find the FarmPlot
        FarmPlot farmPlot = FarmPlot.GetFarmPlotFromRay();
        if (farmPlot)
        {
            farmPlot.AddFertilizer(FertilizerTypeEnum.LOW_QUALITY);
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        // Connect to Event
        G4CInputManager.RegisterInteract(InteractionType.PLANT, OnFertilizerPouchUse);
    }

    private void OnDisable()
    {
        G4CInputManager.RemoveInteract(InteractionType.PLANT, OnFertilizerPouchUse);
    }
}
