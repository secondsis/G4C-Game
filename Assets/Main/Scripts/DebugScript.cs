using System;
using TMPro;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    private static bool DebugMode = true;
    
    public FarmPlot farmPlot;
    public PlayerStatManager _playerStatManager;
    public TextMeshProUGUI _moneyText;
    private const String Symbol = "$";
    
    private void Start()
    {
        farmPlot.PlantCrop(SeedEnum.APPLE);
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
}
