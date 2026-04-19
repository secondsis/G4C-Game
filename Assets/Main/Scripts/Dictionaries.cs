using System;
using System.Collections.Generic;
using UnityEngine;

public static class Dictionaries
{
    public static readonly float DefaultWaterDuration = 300f;
    // public static readonly GameObject WateredPlotObject = Resources.Load<GameObject>("Prefabs/WetPlot");
    // public static readonly GameObject DryPlotObject = Resources.Load<GameObject>("Prefabs/DryPlot");
    public static readonly Material WateredPlotMaterial = Resources.Load<Material>("Materials/WetPlot");
    public static readonly Material DryPlotMaterial = Resources.Load<Material>("Materials/DryPlot");
    public static readonly Dictionary<SeedEnum, float> DefaultPlantGrowthTimes = new()
    {
        { SeedEnum.APPLE, 10f},
        { SeedEnum.CARROT, 30f},
        { SeedEnum.GRAPE, 500f},
        { SeedEnum.STRAWBERRY, 100f},
        { SeedEnum.WATERMELON, 500f},
        { SeedEnum.EGGPLANT, 450f},
        { SeedEnum.CORN, 300f},
        { SeedEnum.TOMATO, 120f},
        { SeedEnum.PUMPKIN, 600f},
        { SeedEnum.TURNIP, 60f},
    };

    // A multiplier on how fast the plant drinks the water. A better watering can will last longer in general.
    public static readonly Dictionary<SeedEnum, float> DefaultPlantThirst = new()
    {
        { SeedEnum.NONE, 1f},
        { SeedEnum.APPLE, 2.4f},
        { SeedEnum.CARROT, 1.3f},
        { SeedEnum.GRAPE, 2f},
        { SeedEnum.STRAWBERRY, 1f},
        { SeedEnum.WATERMELON, 4.3f},
        { SeedEnum.EGGPLANT, 1.8f},
        { SeedEnum.CORN, 1.9f},
        { SeedEnum.TOMATO, 2.1f},
        { SeedEnum.PUMPKIN, 3f},
        { SeedEnum.TURNIP, 1.3f},
    };

    public static readonly Dictionary<SeedEnum, (GameObject, GameObject, GameObject, GameObject)> PlantPrefabs = new()
    {
        { SeedEnum.NONE, (null, null, null, null)},
        { SeedEnum.APPLE, (Resources.Load<GameObject>("Prefabs/ApplePlant").transform.Find("Stage0").gameObject, Resources.Load<GameObject>("Prefabs/ApplePlant").transform.Find("Stage1").gameObject, Resources.Load<GameObject>("Prefabs/ApplePlant").transform.Find("Stage2").gameObject, Resources.Load<GameObject>("Prefabs/ApplePlant").transform.Find("Stage3").gameObject)},
        { SeedEnum.CARROT, (Resources.Load<GameObject>("Prefabs/CarrotPlant").transform.Find("Stage0").gameObject, Resources.Load<GameObject>("Prefabs/CarrotPlant").transform.Find("Stage1").gameObject, Resources.Load<GameObject>("Prefabs/CarrotPlant").transform.Find("Stage2").gameObject, Resources.Load<GameObject>("Prefabs/CarrotPlant").transform.Find("Stage3").gameObject)},
        { SeedEnum.GRAPE, (null, null, null, null)},
        { SeedEnum.STRAWBERRY, (null, null, null, null)},
        { SeedEnum.WATERMELON, (null, null, null, null)},
        { SeedEnum.EGGPLANT, (Resources.Load<GameObject>("Prefabs/EggplantPlant").transform.Find("Stage0").gameObject, Resources.Load<GameObject>("Prefabs/EggplantPlant").transform.Find("Stage1").gameObject, Resources.Load<GameObject>("Prefabs/EggplantPlant").transform.Find("Stage2").gameObject, Resources.Load<GameObject>("Prefabs/EggplantPlant").transform.Find("Stage3").gameObject)},
        { SeedEnum.CORN, (Resources.Load<GameObject>("Prefabs/CornPlant").transform.Find("Stage0").gameObject, Resources.Load<GameObject>("Prefabs/CornPlant").transform.Find("Stage1").gameObject, Resources.Load<GameObject>("Prefabs/CornPlant").transform.Find("Stage2").gameObject, Resources.Load<GameObject>("Prefabs/CornPlant").transform.Find("Stage3").gameObject)},
        { SeedEnum.TOMATO, (Resources.Load<GameObject>("Prefabs/TomatoPlant").transform.Find("Stage0").gameObject, Resources.Load<GameObject>("Prefabs/TomatoPlant").transform.Find("Stage1").gameObject, Resources.Load<GameObject>("Prefabs/TomatoPlant").transform.Find("Stage2").gameObject, Resources.Load<GameObject>("Prefabs/TomatoPlant").transform.Find("Stage3").gameObject)},
        { SeedEnum.PUMPKIN, (Resources.Load<GameObject>("Prefabs/PumpkinPlant").transform.Find("Stage0").gameObject, Resources.Load<GameObject>("Prefabs/PumpkinPlant").transform.Find("Stage1").gameObject, Resources.Load<GameObject>("Prefabs/PumpkinPlant").transform.Find("Stage2").gameObject, Resources.Load<GameObject>("Prefabs/PumpkinPlant").transform.Find("Stage3").gameObject)},
        { SeedEnum.TURNIP, (Resources.Load<GameObject>("Prefabs/TurnipPlant").transform.Find("Stage0").gameObject, Resources.Load<GameObject>("Prefabs/TurnipPlant").transform.Find("Stage1").gameObject, Resources.Load<GameObject>("Prefabs/TurnipPlant").transform.Find("Stage2").gameObject, Resources.Load<GameObject>("Prefabs/TurnipPlant").transform.Find("Stage3").gameObject)},

    };

    public static readonly Dictionary<FertilizerTypeEnum, GameObject> FertilizerPrefabs = new()
    {
        { FertilizerTypeEnum.NONE, null},
        { FertilizerTypeEnum.LOW_QUALITY, Resources.Load<GameObject>("Prefabs/FertilizerLow")},
        { FertilizerTypeEnum.ADEQUATE_QUALITY, Resources.Load<GameObject>("Prefabs/FertilizerMed")},
        { FertilizerTypeEnum.HIGH_QUALITY, Resources.Load<GameObject>("Prefabs/FertilizerHigh")}
    };

    public static readonly Dictionary<FertilizerTypeEnum, float> FertilizerGrowthMultiplier = new()
    {
        { FertilizerTypeEnum.NONE, 1f},
        { FertilizerTypeEnum.LOW_QUALITY, 1.2f},
        { FertilizerTypeEnum.ADEQUATE_QUALITY, 1.5f},
        { FertilizerTypeEnum.HIGH_QUALITY, 2f}
    };
    
    // Increases the chances of higher quality crops (TODO: MAKE DIFFERENT QUALITY CROPS?)
    // High quality fertilizer unlocks a chance of obtaining Tier 4 crops
    public static readonly Dictionary<FertilizerTypeEnum, float> FertilizerQualityMultiplier = new()
    {
        { FertilizerTypeEnum.NONE, 1f},
        { FertilizerTypeEnum.LOW_QUALITY, 1f},
        { FertilizerTypeEnum.ADEQUATE_QUALITY, 1.2f},
        { FertilizerTypeEnum.HIGH_QUALITY, 2f}
    };
    
    // true for when you listened to Wise Joe talk these events
    public static Dictionary<String, Boolean> WiseJoeTalks = new()
    {
        { "Kevin", false }, // KevinsComeback -> JoeWhosKevin
        { "Factory", false }, // FactoryInteract -> JoeHatesPollution
        { "Trucks", false }, // Truckfire -> JoeWhatTrucks
        { "Church", false }, // ChurchInteract -> JoeChurch
        { "Farm", false }, // Obtained when you finish Wise Joe's quests
        { "DirtHoles", false }, // DirtHoles -> JoeWhyDirt
        { "Pigs", false }, // PigInteract -> JoeWhyPigs
        { "Chairs", false }, // ChairGod -> JoeWhatTheChair
        { "Empty", false},
        
    };
}
