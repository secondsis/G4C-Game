using System;
using Main.Scripts;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    private Camera mainCam;

    private void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void OnWateringCanUse()
    {
        // Find the FarmPlot that the mouse is pointing at and if it is within reaching distance
        // Raycasting?
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int plotMask = LayerMask.GetMask("FarmPlotLayer");
    
        // outputs the first object it hits
        // need masks
        if (Physics.Raycast(ray, out hit, PlayerStatManager.ReachDistance, plotMask))
        {
            GameObject obj = hit.transform.gameObject;
            FarmPlot farmPlot = obj.GetComponent<FarmPlot>();
            if (farmPlot)
            {
                // Water this farmplot
                farmPlot.WaterCrop();
            }
            
        }
    }
    
    private void OnEnable()
    {
        G4CInputManager.RegisterInteract(InteractionType.WATER, OnWateringCanUse);
        // Events.OnToolUse += OnWateringCanUse;
    }

    private void OnDisable()
    {
        // Events.OnToolUse -= OnWateringCanUse;
    }
}