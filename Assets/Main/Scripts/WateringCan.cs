using Main.Scripts;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    private Camera mainCam;
    private float _waterCapacity; // May be upgraded later in an upgrade shop
    private float _waterQuantity;

    private void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _waterCapacity = 100f;
        _waterQuantity = 100f;
    }

    // Must wait a bit (waiting prompt?)
    private void RefillWater()
    {
        _waterQuantity = _waterCapacity;
    }
    // Activate the prompt for the water refill at the well
    private void ActivateRefillPrompt()
    {
        
    }
    

    private void OnWateringCanUse()
    {
        // Find the FarmPlot that the mouse is pointing at and if it is within reaching distance
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(ray.origin, ray.direction * PlayerStatManager.ReachDistance, Color.red);
        
        RaycastHit hit;

        int plotMask = LayerMask.GetMask("FarmPlotLayer");
    
        // outputs the first object it hits
        // need masks
        if (Physics.Raycast(ray, out hit, PlayerStatManager.ReachDistance, plotMask))
        {
            GameObject obj = hit.transform.gameObject;
            FarmPlot farmPlot = obj.transform.parent.parent.GetComponent<FarmPlot>();
            if (farmPlot)
            {
                // Water this farmplot
                farmPlot.WaterCrop();
                DebugScript.BetterDebug("Watered crop!");
            }
            
        }
    }
    
    private void OnEnable()
    {
        DebugScript.BetterDebug("Registed watering can");
        G4CInputManager.RegisterInteract(InteractionType.WATER, OnWateringCanUse);
        // Events.OnToolUse += OnWateringCanUse;
    }

    private void OnDisable()
    {
        DebugScript.BetterDebug("Unregisted watering can");
        G4CInputManager.RemoveInteract(InteractionType.WATER, OnWateringCanUse);
        // Events.OnToolUse -= OnWateringCanUse;
    }
}