using System;
using TMPro;
using UnityEngine;

public class CurrencyDisplayer : MonoBehaviour
{
    private PlayerStatManager _playerStatManager;
    
    private TextMeshProUGUI moneyTxt;
    
    private static readonly string DollarSymbol = "$";
    
    private void Awake()
    {
        _playerStatManager = GameObject.FindGameObjectWithTag("PlayerStatManager").GetComponent<PlayerStatManager>();
        moneyTxt = transform.Find("Money").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        moneyTxt.text = DollarSymbol + _playerStatManager.Money;
    }
}
