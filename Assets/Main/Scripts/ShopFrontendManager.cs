using System;
using System.Collections.Generic;
using Main.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopFrontendManager : MonoBehaviour
{
    private static GameObject _shopUI;
    private static Image _profile;
    private static TextMeshProUGUI _shopDialogue;
    
    private static readonly Dictionary<string, Sprite> ShopOwnerDictionary = new Dictionary<string, Sprite>();

    private void Awake()
    {
        _shopUI = transform.Find("UI").gameObject;
        _profile = _shopUI.transform.Find("Profile").Find("Background").Find("Person").GetComponent<Image>();
        _shopDialogue = _shopUI.transform.Find("Dialogue").Find("Text").GetComponent<TextMeshProUGUI>();
        ShopOwnerDictionary.Add("Kevin", Resources.Load<Sprite>("Icons/Kevin"));
    }

    public static void OpenShop(string shopOwner, string msg)
    {
        // Set the name/pfp of the shop owner, and set the msg textugui
        _profile.sprite = ShopOwnerDictionary[shopOwner];
        _shopDialogue.text = msg;
        _shopUI.SetActive(true);
    }
    
    public static void CloseShop()
    {
        _shopUI.SetActive(false);
        DialogueUI.LaunchDialogue("SeedShopLeave1_0");
    }
}
