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
        { SeedEnum.NONE, 100f},
        { SeedEnum.APPLE, 2f},
        { SeedEnum.CARROT, 2f},
        { SeedEnum.GRAPE, 1.3f},
        { SeedEnum.STRAWBERRY, 1f},
        { SeedEnum.WATERMELON, 4.3f},
        { SeedEnum.EGGPLANT, 2f},
        { SeedEnum.CORN, 2f},
        { SeedEnum.TOMATO, 2f},
        { SeedEnum.PUMPKIN, 2f},
        { SeedEnum.TURNIP, 2f},
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
        { FertilizerTypeEnum.LOW_QUALITY, null},
        { FertilizerTypeEnum.ADEQUATE_QUALITY, null},
        { FertilizerTypeEnum.HIGH_QUALITY, null}
    };
}
