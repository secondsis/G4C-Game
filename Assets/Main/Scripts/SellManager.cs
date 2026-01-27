using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellManager : MonoBehaviour
{
    public static event Action OnSellMenuOpen;
    public static event Action OnSellMenuClose;
    
    [SerializeField] private GameObject sellMenuSlotPrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private Transform itemPanel;
    
    [SerializeField] private List<ShopItem> sellPrices;
    
    private int _itemPanelHashCode;

    private GameObject _ui;

    private bool _inventoryBlocking = false;
    private bool _shopBlocking = false;
    
    private void Awake()
    {
        _ui = gameObject.transform.parent.Find("UI").gameObject;
        _ui.SetActive(false);
        
        
    }

    private void SetInventoryBlockingTrue()
    {
        _inventoryBlocking = true;
    }
    private void SetInventoryBlockingFalse()
    {
        _inventoryBlocking = false;
    }
    private void SetShopBlockingTrue()
    {
        _shopBlocking = true;
    }
    private void SetShopBlockingFalse()
    {
        _shopBlocking = false;
    }

    private void OnEnable()
    {
        // Blocking menus
        InventoryManager.OnInventoryOpen += SetInventoryBlockingTrue;
        InventoryManager.OnInventoryClose += SetInventoryBlockingFalse;

        ShopFrontendManager.OnShopOpen += SetShopBlockingTrue;
        ShopFrontendManager.OnShopClose += SetShopBlockingFalse;
    }

    private void OnDisable()
    {
        InventoryManager.OnInventoryOpen -= SetInventoryBlockingTrue;
        InventoryManager.OnInventoryClose -= SetInventoryBlockingFalse;
        
        ShopFrontendManager.OnShopOpen -= SetShopBlockingTrue;
        ShopFrontendManager.OnShopClose -= SetShopBlockingFalse;
    }

    public void OpenSellMenu()
    {
        if (_inventoryBlocking || _shopBlocking)
        {
            DebugScript.BetterDebug("WARNING: Cannot open sell menu because inventory or shop are open");
            return;
        }
        UpdateSellMenu();
        _ui.SetActive(true);
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
        OnSellMenuOpen?.Invoke();
    }

    public void CloseSellMenu()
    {
        _ui.SetActive(false);
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        OnSellMenuClose?.Invoke();
    }

    private void SellItem(InventoryItem item, int quantityToRemove)
    {
        // Take from item dictionary to find the sell amount
        ShopItem itemFromShop = sellPrices.Find(n => n.itemID.Equals(item.Item.ItemName));
        if (itemFromShop != null)
        {
            PlayerStatManager.Instance.AddMoney(itemFromShop.cost * quantityToRemove);
        
            InventoryManager.Instance.RemoveItem(item, quantityToRemove);
            UpdateSellMenu();
            ResetItemPanel();
        }
    }
    
    public void SellItemOnce() 
    {
        InventoryItem ii = InventoryManager.Instance.GetInventoryItemFromHashCode(_itemPanelHashCode);
        SellItem(ii, 1);
    }

    public void SellAllItem()
    {
        InventoryItem ii = InventoryManager.Instance.GetInventoryItemFromHashCode(_itemPanelHashCode);
        SellItem(ii, ii.Quantity);
    }

    public void UpdateSellMenu()
    {
        // Clear menu first
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        
        List<InventoryItem> inventory = InventoryManager.Instance.GetInventory();
        // Only can sell items that are PRODUCT
        // Any others must be blacked/grayed out
        foreach (InventoryItem inventoryItem in inventory)
        {
            GameObject slot = Instantiate(sellMenuSlotPrefab, content.transform);
            // Paint the image and quantity txt for "inventory" slot
            
            int quantity = inventoryItem.Quantity;
            Sprite sprite = inventoryItem.Item.Sprite;
            
            Transform quantityObj = slot.transform.Find("Quantity");
            Transform imageObj = slot.transform.Find("Image");
            TextMeshProUGUI quantityTxt = quantityObj.GetComponent<TextMeshProUGUI>();
            Image image = imageObj.GetComponent<Image>();

            quantityTxt.text = "x" + quantity;
            quantityTxt.enabled = true;
            image.sprite = sprite;
            image.enabled = true;
            
            if (inventoryItem.Item.ItemType == ItemTypeEnum.PRODUCT)
            {
                // Set the button onClick
                Button btn =  slot.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    // Check if we should close itemPanel
                    if (itemPanel.gameObject.activeSelf && _itemPanelHashCode == inventoryItem.GetHashCode())
                    {
                        itemPanel.gameObject.SetActive(false);
                        return;
                    }
                    // Open up itemPanel
                    _itemPanelHashCode = inventoryItem.GetHashCode();
                    Image img = itemPanel.Find("IMAGE").GetComponent<Image>();
                    TextMeshProUGUI nameTxt = itemPanel.Find("NameBG").Find("NAME").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI descTxt = itemPanel.transform.Find("DescBG").Find("DESCRIPTION").GetComponent<TextMeshProUGUI>();

                    img.sprite = sprite;
                    img.enabled = true;
                    
                    nameTxt.text = inventoryItem.Item.ItemName;
                    nameTxt.enabled = true;
                    
                    ShopItem itemFromShop = sellPrices.Find(n => n.itemID.Equals(inventoryItem.Item.ItemName));
                    if (itemFromShop != null)
                    {
                        descTxt.text = "$" + itemFromShop.cost + " ea.";
                    }
                    else
                    {
                        descTxt.text = "Not available";
                    }

                    descTxt.enabled = true;
                    
                    itemPanel.gameObject.SetActive(true);
                });
            }
            else
            {
                GrayOutSlot(slot);
            }
        }
    }

    private void GrayOutSlot(GameObject slot)
    {
        slot.transform.Find("Panel").GetComponent<Image>().enabled = true;
    }
    
    private void ResetItemPanel()
    {
        Image img = itemPanel.Find("IMAGE").GetComponent<Image>();
        TextMeshProUGUI nameTxt = itemPanel.Find("NameBG").Find("NAME").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descTxt = itemPanel.transform.Find("DescBG").Find("DESCRIPTION").GetComponent<TextMeshProUGUI>();
        
        itemPanel.gameObject.SetActive(false);
        _itemPanelHashCode = -1;
        img.enabled = false;
        nameTxt.text = "";
        nameTxt.enabled = false;
        descTxt.text = "";
        descTxt.enabled = false;
    }
}
