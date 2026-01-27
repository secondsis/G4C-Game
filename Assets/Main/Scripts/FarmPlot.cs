using System;
using Main.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

public class FarmPlot : MonoBehaviour
{
    // TODO: Fix carrots no longer harvesting. Goal: Allow carrot seeds to be planted
    public FarmLogic Logic = new FarmLogic();
    private int _currentPlantStage;
    private bool _plotWatered;
    private Transform _cropParent;
    private Hover3DTooltip _tooltip;
    private static readonly String _defaultLeftInfo = "Unfertilized Plot\nAn empty area of farmable land.";
    private static readonly String _defaultRightInfo = "(EMPTY)";
    private static readonly String _fertilizedLeftInfo = "Fertilized Plot\nAn empty area of farmable land.";
    private bool _tooltipShowing;

    private void Awake()
    {
        _cropParent = transform.Find("Crop");
        _tooltip = transform.Find("Plot").Find("PlotObject").GetComponent<Hover3DTooltip>();
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
// Idk why but tomato is turned into carrot
    public bool PlantCrop(SeedEnum seed)
    {
        if (!Logic.PlantCrop(seed)) return false;
        
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
            Object.Destroy(child.gameObject);
        }
        // Fertilizer Texture
        Instantiate(Dictionaries.FertilizerPrefabs[fert], fertParent);
        return true;
    }

    public void WaterCrop()
    {
        Transform plotParent = gameObject.transform.Find("Plot");
        foreach (Transform child in plotParent)
        {
            Destroy(child.gameObject);
        }

        // Watered Texture -- MIGHT BE UNFINISHED???
        GameObject newObj = Instantiate(Dictionaries.WateredPlotObject, plotParent);
        _plotWatered = true;
    }

    private void SetPlantStage(int stage)
    {
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
        // WIP
    }

    // Check for water and plant stage
    // Is this efficient?
    private void Update()
    {
        if (!Logic.IsWatered() && _plotWatered)
        {
            DryPlot();
        }

        if (Logic.GetPlantStage() != _currentPlantStage)
        {
            SetPlantStage(Logic.GetPlantStage());
        }
        
        // Check if player is hovering over plot, if so then check for E to harvest
        // if (_tooltip.showing)
        // {
        //     // Change this to an OnInteract
        //     
        //     // if (Input.GetKeyDown(KeyCode.E))
        //     // {
        //     //     HarvestCrop();
        //     // }
        // }
    }
}

public class FarmLogic
{
    public FertilizerTypeEnum Fertilizer { get; private set; }
    public SeedEnum SeedType { get; private set; }
    public long UnixTimePlanted { get; private set; }

    public long UnixTimeLastWatered { get; private set; }

    public FarmLogic()
    {
        Fertilizer = FertilizerTypeEnum.NONE;
        SeedType = SeedEnum.NONE;
        UnixTimeLastWatered = 0L;
    }

    public bool PlantCrop(SeedEnum seed)
    {
        if (SeedType != SeedEnum.NONE) return false;
        SeedType = seed;
        UnixTimePlanted = GlobalTime.UnixTime;
        return true;
    }

    public SeedEnum HarvestCrop()
    {
        if (SeedType == SeedEnum.NONE || GetPlantStage() != 3) return SeedEnum.NONE;
        SeedEnum oldSeed = SeedType;
        SeedType = SeedEnum.NONE;
        Fertilizer = FertilizerTypeEnum.NONE;
        UnixTimePlanted = 0;
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
        UnixTimeLastWatered = GlobalTime.UnixTime;
    }

    public bool IsWatered()
    {
        // If seconds past from last time watered is greater than the plant's water duration, then it is not watered
        if (GlobalTime.UnixTime - UnixTimeLastWatered >= Dictionaries.DefaultWaterDuration * Dictionaries.DefaultPlantThirst[SeedType])
        {
            return false;
        }
        return true;
    }

    public int GetPlantStage()
    {
        if (SeedType == SeedEnum.NONE)
        {
            return 0;
        }

        long timeGrown = GlobalTime.UnixTime - UnixTimePlanted;
        float plantGrowthTime = Dictionaries.DefaultPlantGrowthTimes[SeedType];

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