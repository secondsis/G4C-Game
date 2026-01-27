using System;
using System.Collections.Generic;
using Main.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopFrontendManager : MonoBehaviour
{
    public static event Action OnShopOpen;
    public static event Action OnShopClose;
    
    private static GameObject _shopUI;
    private static Image _profile;
    private static TextMeshProUGUI _shopDialogue;
    
    private static readonly Dictionary<string, Sprite> ShopOwnerDictionary = new Dictionary<string, Sprite>();
    
    // Menu Blocking
    private static bool _sellMenuBlocking = false;
    private static bool _inventoryBlocking = false;

    private void Awake()
    {
        _shopUI = transform.Find("UI").gameObject;
        _profile = _shopUI.transform.Find("Profile").Find("Background").Find("Person").GetComponent<Image>();
        _shopDialogue = _shopUI.transform.Find("Dialogue").Find("Text").GetComponent<TextMeshProUGUI>();
        ShopOwnerDictionary.Add("Kevin", Resources.Load<Sprite>("Icons/kevin-alpha"));
    }
    
    private void SetSellMenuBlockingTrue()
    {
        _sellMenuBlocking = true;
    }
    private void SetSellMenuBlockingFalse()
    {
        _sellMenuBlocking = false;
    }
    private void SetInventoryBlockingTrue()
    {
        _inventoryBlocking = true;
    }
    private void SetInventoryBlockingFalse()
    {
        _inventoryBlocking = false;
    }

    private void OnEnable()
    {
        SellManager.OnSellMenuOpen += SetSellMenuBlockingTrue;
        SellManager.OnSellMenuClose += SetSellMenuBlockingFalse;
        
        InventoryManager.OnInventoryOpen += SetInventoryBlockingTrue;
        InventoryManager.OnInventoryClose += SetInventoryBlockingFalse;
    }

    private void OnDisable()
    {
        SellManager.OnSellMenuOpen -= SetSellMenuBlockingTrue;
        SellManager.OnSellMenuClose -= SetSellMenuBlockingFalse;

        InventoryManager.OnInventoryOpen -= SetInventoryBlockingTrue;
        InventoryManager.OnInventoryClose -= SetInventoryBlockingFalse;
    }

    public static void OpenShop(string shopOwner, string msg)
    {
        if (_inventoryBlocking || _sellMenuBlocking)
        {
            DebugScript.BetterDebug("WARNING: Cannot open shop because inventory or sell menu are open");
            return;
        }
        // Set the name/pfp of the shop owner, and set the msg textugui
        _profile.sprite = ShopOwnerDictionary[shopOwner];
        _shopDialogue.text = msg;
        _shopUI.SetActive(true);
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
        OnShopOpen?.Invoke();
        SFXManager.Instance.ShopEnterSfx();
    }
    
    public static void CloseShop()
    {
        _shopUI.SetActive(false);
        DialogueUI.LaunchDialogue("SeedShopLeave1_0");
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        OnShopClose?.Invoke();
    }
}
