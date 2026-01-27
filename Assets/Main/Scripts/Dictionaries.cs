using System.Collections.Generic;
using UnityEngine;

public static class Dictionaries
{
    public static readonly float DefaultWaterDuration = 300f;
    public static readonly GameObject WateredPlotObject = null;
    public static readonly Dictionary<SeedEnum, float> DefaultPlantGrowthTimes = new()
    {
        { SeedEnum.APPLE, 10f},
        { SeedEnum.CARROT, 10f},
        { SeedEnum.GRAPE, 500f},
        { SeedEnum.STRAWBERRY, 100f},
        { SeedEnum.WATERMELON, 500f}
    };

    public static readonly Dictionary<SeedEnum, float> DefaultPlantThirst = new()
    {
        { SeedEnum.NONE, 100f},
        { SeedEnum.APPLE, 2f},
        { SeedEnum.CARROT, 2f},
        { SeedEnum.GRAPE, 1.3f},
        { SeedEnum.STRAWBERRY, 1f},
        { SeedEnum.WATERMELON, 4.3f}
    };

    public static readonly Dictionary<SeedEnum, (GameObject, GameObject, GameObject, GameObject)> PlantPrefabs = new()
    {
        { SeedEnum.NONE, (null, null, null, null)},
        { SeedEnum.APPLE, (Resources.Load<GameObject>("Prefabs/ApplePlant").transform.Find("Stage0").gameObject, Resources.Load<GameObject>("Prefabs/ApplePlant").transform.Find("Stage1").gameObject, Resources.Load<GameObject>("Prefabs/ApplePlant").transform.Find("Stage2").gameObject, Resources.Load<GameObject>("Prefabs/ApplePlant").transform.Find("Stage3").gameObject)},
        { SeedEnum.CARROT, (Resources.Load<GameObject>("Prefabs/CarrotPlant").transform.Find("Stage0").gameObject, Resources.Load<GameObject>("Prefabs/CarrotPlant").transform.Find("Stage1").gameObject, Resources.Load<GameObject>("Prefabs/CarrotPlant").transform.Find("Stage2").gameObject, Resources.Load<GameObject>("Prefabs/CarrotPlant").transform.Find("Stage3").gameObject)},
        { SeedEnum.GRAPE, (null, null, null, null)},
        { SeedEnum.STRAWBERRY, (null, null, null, null)},
        { SeedEnum.WATERMELON, (null, null, null, null)}
    };

    public static readonly Dictionary<FertilizerTypeEnum, GameObject> FertilizerPrefabs = new()
    {
        { FertilizerTypeEnum.NONE, null},
        { FertilizerTypeEnum.LOW_QUALITY, null},
        { FertilizerTypeEnum.ADEQUATE_QUALITY, null},
        { FertilizerTypeEnum.HIGH_QUALITY, null}
    };

    // public static readonly Dictionary<SeedEnum, float> SeedCosts = new()
    // {
    //     { SeedEnum.APPLE, 10.00f},
    //     { SeedEnum.CARROT, 2.00f},
    //     { SeedEnum.GRAPE, 5.00f},
    //     { SeedEnum.STRAWBERRY, 2.50f},
    //     { SeedEnum.WATERMELON, 12.00f}
    // };

}
