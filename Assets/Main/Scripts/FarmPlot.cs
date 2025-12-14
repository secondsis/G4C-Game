using UnityEngine;

public class FarmPlot : MonoBehaviour
{
    public FarmInfo farmInfo;
    private int currentPlantStage;
    private bool plotWatered;

    public FarmPlot(FarmInfo info)
    {
        farmInfo = info;
    }

    public bool PlantCrop(SeedEnum seed)
    {
        if (!farmInfo.plantCrop(seed)) return false;
        Transform cropParent = gameObject.transform.Find("Crop");
        foreach (Transform child in cropParent)
        {
            Object.Destroy(child.gameObject);
        }
        // Baby stage crop
        Object.Instantiate(PlantDictionaries.PlantPrefabs[seed].Item1, cropParent.position, cropParent.rotation);
        currentPlantStage = 0;
        return true;
    }

    public bool AddFertilizer(FertilizerTypeEnum fert)
    {
        if (!farmInfo.addFertilizer(fert)) return false;
        Transform fertParent = gameObject.transform.Find("Fertilizer");
        foreach (Transform child in fertParent)
        {
            Object.Destroy(child.gameObject);
        }
        // Fertilizer Texture
        Object.Instantiate(PlantDictionaries.FertilizerPrefabs[fert], fertParent.position, fertParent.rotation);
        return true;
    }

    public void WaterCrop()
    {
        Transform plotParent = gameObject.transform.Find("Plot");
        foreach (Transform child in plotParent)
        {
            Object.Destroy(child.gameObject);
        }

        // Watered Texture
        GameObject newObj = Object.Instantiate(PlantDictionaries.WateredPlotObject, plotParent.position, plotParent.rotation);
        plotWatered = true;
    }

    private void SetPlantStage(int stage)
    {
        Transform cropParent = gameObject.transform.Find("Crop");
        foreach (Transform child in cropParent)
        {
            Destroy(child.gameObject);
        }

        GameObject obj = null;
        switch(stage)
        {
            case 0:
                obj = PlantDictionaries.PlantPrefabs[farmInfo.seedType].Item1;
                break;
            case 1: 
                obj = PlantDictionaries.PlantPrefabs[farmInfo.seedType].Item2;
                break;
            case 2: 
                obj = PlantDictionaries.PlantPrefabs[farmInfo.seedType].Item3;
                break;
            case 3: 
                obj = PlantDictionaries.PlantPrefabs[farmInfo.seedType].Item4;
                break;
        }

        Instantiate(obj, cropParent.position, cropParent.rotation);
        currentPlantStage = stage;
    }

    private void DryPlot()
    {
        plotWatered = false;
        // WIP
    }

    // Check for water and plant stage
    // Is this efficient?
    private void Update()
    {
        if (!farmInfo.isWatered() && plotWatered)
        {
            DryPlot();
        }

        if (farmInfo.getPlantStage() != currentPlantStage)
        {
            SetPlantStage(farmInfo.getPlantStage());
        }
    }
}

public class FarmInfo
{
    public FertilizerTypeEnum fertilizer { get; private set; }
    public SeedEnum seedType { get; private set; }
    public long unixTimePlanted { get; private set; }

    public long unixTimeLastWatered { get; private set; }

    public FarmInfo(GameObject obj)
    {
        fertilizer = FertilizerTypeEnum.NONE;
        seedType = SeedEnum.NONE;
        unixTimeLastWatered = 0L;
    }

    public bool plantCrop(SeedEnum seed)
    {
        if (seedType == SeedEnum.NONE) return false;
        seedType = seed;
        unixTimePlanted = GlobalTime.UnixTime;
        return true;
    }

    public bool addFertilizer(FertilizerTypeEnum fert)
    {
        if (fertilizer != FertilizerTypeEnum.NONE) return false;
        fertilizer = fert;
        return true;
    }

    public void waterCrop()
    {
        unixTimeLastWatered = GlobalTime.UnixTime;
    }

    public bool isWatered()
    {
        // If seconds past from last time watered is greater than the plant's water duration, then it is not watered
        if (GlobalTime.UnixTime - unixTimeLastWatered >= PlantDictionaries.DefaultWaterDuration * PlantDictionaries.DefaultPlantThirst[seedType])
        {
            return false;
        }
        return true;
    }

    public int getPlantStage()
    {
        if (seedType == SeedEnum.NONE)
        {
            return 0;
        }

        long timeGrown = GlobalTime.UnixTime - unixTimePlanted;
        float plantGrowthTime = PlantDictionaries.DefaultPlantGrowthTimes[seedType];

        if (timeGrown <= 0.25f * plantGrowthTime)
        {
            return 0;
        }
        else if (timeGrown <= 0.5f * plantGrowthTime)
        {
            return 1;
        }
        else if (timeGrown <= 0.75f * plantGrowthTime)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
}