using System;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public FarmPlot farmPlot;

    private void Start()
    {
        farmPlot.PlantCrop(SeedEnum.APPLE);
    }
}
