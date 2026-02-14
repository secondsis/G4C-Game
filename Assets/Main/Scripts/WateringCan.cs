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
        DebugScript.BetterDebug("Refilled Watering Can (" +  _waterQuantity + " liters)");
    }

    private void OnWateringCanUse()
    {
        if (_waterQuantity - 1 < 0) return;
        
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
                _waterQuantity -= 1;
                DebugScript.BetterDebug("Used Watering Can (" +  _waterQuantity + " liters)");
            }
            
        }
    }
    
    private void OnEnable()
    {
        DebugScript.BetterDebug("Registed watering can");
        // Register an Interact2 for using the watering can (F)
        // These two are competing for each other (if they are the same interact button)
        G4CInputManager.RegisterInteract(InteractionType.WATER, OnWateringCanUse, 1);
        // Connect to Refill interact
        Events.OnWaterWellInteract += RefillWater;
    }

    private void OnDisable()
    {
        DebugScript.BetterDebug("Unregisted watering can");
        G4CInputManager.RemoveInteract(InteractionType.WATER, OnWateringCanUse, 1);
        Events.OnWaterWellInteract -= RefillWater;
    }
}