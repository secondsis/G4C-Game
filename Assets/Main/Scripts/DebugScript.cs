using System;
using Main.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DebugScript : MonoBehaviour
{
    private static bool DebugMode = true;
    
    public FarmPlot farmPlot;
    public PlayerStatManager _playerStatManager;
    public TextMeshProUGUI _moneyText;
    [FormerlySerializedAs("poucPrefab")] public GameObject pouchPrefab;
    private const String Symbol = "$";
    
    private void Start()
    {
        InventoryManager.Instance.AddItem("watering-can", 1);
    }

    private void Update()
    {
        _moneyText.text = Symbol + _playerStatManager.Money;
    }

    public static void BetterDebug(System.Object message)
    {
        #if UNITY_EDITOR
        if (!DebugMode) return;
        Debug.Log(message);
        #endif
    }

    // Temporary to debug items (not watering can)
    public static void AddWateringCan()
    {
        InventoryManager.Instance.AddItem("fertilizer-pouch", 1);
        
    }
}
