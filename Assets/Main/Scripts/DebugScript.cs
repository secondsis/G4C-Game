using System;
using TMPro;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
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
}
