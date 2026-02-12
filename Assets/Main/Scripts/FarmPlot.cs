using System;
using System.Collections.Generic;
using Main.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

public class FarmPlot : MonoBehaviour
{
    // TODO: Fix a bug with crops.
    public FarmLogic Logic = new FarmLogic();
    private int _currentPlantStage;
    private bool _plotWatered;
    private Transform _cropParent;
    private Transform _plotParent;
    private Renderer _plotObjectRenderer;
    private Hover3DTooltip _tooltip;
    private static readonly String _defaultLeftInfo = "Unfertilized Plot\nAn empty area of farmable land.";
    private static readonly String _defaultRightInfo = "(EMPTY)";
    private static readonly String _fertilizedLeftInfo = "Fertilized Plot\nAn empty area of farmable land.";
    private bool _tooltipShowing;

    private void Awake()
    {
        _cropParent = transform.Find("Crop");
        _plotParent = transform.Find("Plot");
        _plotObjectRenderer = _plotParent.GetChild(0).GetComponent<Renderer>();
        _tooltip = transform.Find("Plot").Find("PlotObject").GetComponent<Hover3DTooltip>();
        DebugScript.BetterDebug(_tooltip);
        _tooltip.infoLeft = _defaultLeftInfo;
        _tooltip.infoRight = _defaultRightInfo;

        // This allows the crops to be harvested. If a seed is equipped, add that function too
        // Go into inventory and check if a item type is seed?
        _tooltip.OnTooltipShow += () =>
        {
            if (_tooltipShowing)
            {
                return;
            }
            _tooltipShowing = true;
            G4CInputManager.RegisterInteract(InteractionType.HARVEST, HarvestCrop);
            InventoryItem currentlyEquipped = InventoryManager.Instance.CurrentlyEquipped;
            if (currentlyEquipped != null && currentlyEquipped.Item.ItemType == ItemTypeEnum.SEED)
            {
                // If interact, plant crop. Remove interact when item is unequipped/finished used
                G4CInputManager.RegisterInteract(InteractionType.PLANT, UseSeed);
            }
        };
        
        _tooltip.OnTooltipHide += () =>
        {
            if (!_tooltipShowing) return;
            _tooltipShowing = false;
            G4CInputManager.RemoveInteract(InteractionType.HARVEST, HarvestCrop);
            // might cause error if no plant interact
            G4CInputManager.RemoveInteract(InteractionType.PLANT, UseSeed);
        };
    }

    private void UseSeed()
    {
        // may be something wrong with currentlyequipped?
        Debug.Log("Using seed: " + InventoryManager.Instance.CurrentlyEquipped.Item.ItemName);
        string plantName = InventoryManager.Instance.CurrentlyEquipped.Item.ItemName.Replace(" Seed", "");
        if (Enum.TryParse(plantName.ToUpper(), out SeedEnum thisSeed))
        {
            bool succ = PlantCrop(thisSeed);
            Debug.Log(thisSeed);
            // Remove a quantity of 1 from the hotbar/inventory
            if(succ)
                InventoryManager.Instance.DecrementCurrentlyEquipped();
        }
        else
        {
            Debug.LogWarning("Could not find seed type!");
        }

    }
// TODO: Fix crops changing state
    public bool PlantCrop(SeedEnum seed)
    {
        
        if (!Logic.PlantCrop(seed)) return false;
        DebugScript.BetterDebug("Planted " + seed);
        
        foreach (Transform child in _cropParent)
        {
            Destroy(child.gameObject);
        }
        // Baby stage crop
        Instantiate(Dictionaries.PlantPrefabs[seed].Item1, _cropParent);
        _currentPlantStage = 0;
        // Update Hover UI
        _tooltip.infoLeft = $"{seed.ToString()}\nThere is a plant here.";
        _tooltip.infoRight = "(GROWING)";
        return true;
    }

    // Has a check in place so this will only complete if the crop is mature
    public void HarvestCrop()
    {
        SeedEnum harvestedSeed = Logic.HarvestCrop();
        if (harvestedSeed == SeedEnum.NONE) return;
        Debug.Log("Harvested " + harvestedSeed);
        // if (harvestedSeed == SeedEnum.NONE) return false;
        foreach (Transform child in _cropParent)
        {
            Destroy(child.gameObject);
        }
        
        if(Logic.Fertilizer == FertilizerTypeEnum.NONE) _tooltip.infoLeft = _defaultLeftInfo;
        else _tooltip.infoRight = _fertilizedLeftInfo;
        
        _tooltip.infoRight = _defaultRightInfo;
        
        InventoryManager.Instance.AddItem(harvestedSeed.ToString().ToLower(), 1);
        _currentPlantStage = 0;
        SFXManager.Instance.HarvestSfx();
        // return true;
    }

    public bool AddFertilizer(FertilizerTypeEnum fert)
    {
        if (!Logic.AddFertilizer(fert)) return false;
        Transform fertParent = gameObject.transform.Find("Fertilizer");
        foreach (Transform child in fertParent)
        {
            Destroy(child.gameObject);
        }
        // Fertilizer Texture
        Instantiate(Dictionaries.FertilizerPrefabs[fert], fertParent);
        return true;
    }

    public void WaterCrop()
    {
        
        // foreach (Transform child in _plotParent)
        // {
        //     Destroy(child.gameObject);
        // }

        // Watered Texture
        _plotObjectRenderer.material = Dictionaries.WateredPlotMaterial;
        // GameObject newObj = Instantiate(Dictionaries.WateredPlotObject, _plotParent);
        _plotWatered = true;
        Logic.WaterCrop();
    }

    private void SetPlantStage(int stage)
    {
        DebugScript.BetterDebug("Setting plant stage: " + stage);
        foreach (Transform child in _cropParent)
        {
            Destroy(child.gameObject);
        }

        GameObject obj = stage switch
        {
            0 => Dictionaries.PlantPrefabs[Logic.SeedType].Item1,
            1 => Dictionaries.PlantPrefabs[Logic.SeedType].Item2,
            2 => Dictionaries.PlantPrefabs[Logic.SeedType].Item3,
            3 => Dictionaries.PlantPrefabs[Logic.SeedType].Item4,
            _ => null
        };

        if (stage == 3)
        {
            _tooltip.infoRight = "(READY)";
        }

        if (obj != null)
        {
            Instantiate(obj, _cropParent);
            _currentPlantStage = stage;
        }
    }

    private void DryPlot()
    {
        _plotWatered = false;
        // Just change material
        _plotObjectRenderer.material = Dictionaries.DryPlotMaterial;
    }

    // Check for water and plant stage
    // TODO: Make this more efficient?
    private void Update()
    {
        Logic.DecrementWater(Time.deltaTime);
        Logic.GrowPlant(Time.deltaTime);
        
        if (!Logic.IsWatered() && _plotWatered)
        {
            DryPlot();
        }

        if (Logic.GetPlantStage() != _currentPlantStage)
        {
            SetPlantStage(Logic.GetPlantStage());
        }
    }
}

public class FarmLogic
{
    public FertilizerTypeEnum Fertilizer { get; private set; }
    public SeedEnum SeedType { get; private set; }

    private float PercentWater;
    private float PercentGrown;

    public FarmLogic()
    {
        Fertilizer = FertilizerTypeEnum.NONE;
        SeedType = SeedEnum.NONE;
        PercentWater = 0f;
    }

    public bool PlantCrop(SeedEnum seed)
    {
        if (SeedType != SeedEnum.NONE) return false;
        PercentGrown = 0f;
        SeedType = seed;
        return true;
    }

    public SeedEnum HarvestCrop()
    {
        if (SeedType == SeedEnum.NONE || GetPlantStage() != 3) return SeedEnum.NONE;
        SeedEnum oldSeed = SeedType;
        SeedType = SeedEnum.NONE;
        Fertilizer = FertilizerTypeEnum.NONE;
        PercentGrown = 0f;
        return oldSeed;
    }

    public bool AddFertilizer(FertilizerTypeEnum fert)
    {
        if (Fertilizer != FertilizerTypeEnum.NONE) return false;
        Fertilizer = fert;
        return true;
    }

    public void WaterCrop()
    {
        PercentWater = 1f;
        // UnixTimeLastWatered = GlobalTime.UnixTime;
        // DebugScript.BetterDebug("Wait for " + (Dictionaries.DefaultWaterDuration * Dictionaries.DefaultPlantThirst[SeedType]) + " time");

    }

    // MUST BE CALLED IN UPDATE()
    public void DecrementWater(float delta)
    {
        if (PercentWater > 0f)
        {
            // Every second is multiplied by the plant's thirst (ex. x1.3 mult.), then divided by the expected water duration
            PercentWater -= delta * Dictionaries.DefaultPlantThirst[SeedType] / Dictionaries.DefaultWaterDuration;

            if (PercentWater < 0f) PercentWater = 0f;
        }
    }

    // simplified
    public bool IsWatered()
    {
        return !(PercentWater <= 0f);
    }

    // MUST CALL IN UPDATE
    public void GrowPlant(float delta)
    {
        if (SeedType == SeedEnum.NONE) return;
        // Can't grow without water
        if (!IsWatered()) return;
        if (PercentGrown >= 1f) return;
        
        // Any growth inhibitors/whatever can be multiplied at the end
        PercentGrown += delta / Dictionaries.DefaultPlantGrowthTimes[SeedType];
        if(PercentGrown > 1f) 
        {
            PercentGrown = 1f;
        }
    }

    public int GetPlantStage()
    {
        if (SeedType == SeedEnum.NONE) return 0;

        // long timeGrown = GlobalTime.UnixTime - UnixTimePlanted;
        // float plantGrowthTime = Dictionaries.DefaultPlantGrowthTimes[SeedType];

        return (int) (PercentGrown / 0.33f);
    }
}