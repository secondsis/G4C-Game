using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public InventoryBackendManager _inventoryBackendManager = new InventoryBackendManager();
    [SerializeField] private GameObject inventorySlotsParent;
    private List<GameObject> _slots = new List<GameObject>();
    private GameObject _slotPrefab;
    [SerializeField] private GameObject hotbarSlotsParent;
    private List<GameObject> _hotbarSlots = new List<GameObject>();
    private GameObject _hotbarSlotPrefab;

    public InventoryItem[] Hotbar = new InventoryItem[10];

    private void Awake()
    {
        _slotPrefab = Resources.Load<GameObject>("Prefabs/InventorySlot");
        InventoryFetcher.Manager = this;
    }

    public void EnableAllTooltips()
    {
        foreach (GameObject slot in _slots)
        {
            slot.GetComponent<Hover2DTooltip>().enableTooltip();
        }
        
        foreach (GameObject slot in _hotbarSlots)
        {
            slot.GetComponent<Hover2DTooltip>().enableTooltip();
        }
    }
    
    public void DisableAllTooltips()
    {
        foreach (GameObject slot in _slots)
        {
            slot.GetComponent<Hover2DTooltip>().disableTooltip();
        }
        
        foreach (GameObject slot in _hotbarSlots)
        {
            slot.GetComponent<Hover2DTooltip>().disableTooltip();
        }
    }

    public void OpenInventory()
    {
        gameObject.transform.parent.gameObject.SetActive(true);
        
        // FUTURE: ADD ANIMATIONS TO OPENING
        
        EnableAllTooltips();
    }

    public void CloseInventory()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        
        // FUTURE: ADD ANIMATIONS
        
        DisableAllTooltips();
    }

    public void EquipItem(string itemCodeName, int quantity)
    {
        for (int i = 0; i < Hotbar.Length; i++)
        {
            if (Hotbar[i] == null)
            {
                Hotbar[i] = new InventoryItem(itemCodeName, quantity);
                break;
            }
        }
    }

    public void UnequipItem(string itemCodeName, int quantity)
    {
        for (int i = 0; i < Hotbar.Length; i++)
        {
            if (Hotbar[i].Equals(new InventoryItem(itemCodeName, quantity)))
            {
                Hotbar[i] = null;
                break;
            }
        }
    }

    public void UpdateHotbar()
    {
        while (_hotbarSlots.Count > Hotbar.Length)
        {
            Destroy(_hotbarSlots[_hotbarSlots.Count - 1]);
            _hotbarSlots.RemoveAt(_hotbarSlots.Count - 1);
        }

        for (int i = 0; i < Hotbar.Length; i++)
        {
            InventoryItem ii = Hotbar[i];

            Item item = ii.Item;
            int quantity = ii.Quantity;

            // Item properties
            string itemName = item.ItemName;
            string desc = item.Description;
            string itemType = item.ItemType.ToString();
            Sprite sprite = item.Sprite;

            GameObject thisSlot;
            if (i < _hotbarSlots.Count)
            {
                thisSlot = _hotbarSlots[i];
            }
            else
            {
                thisSlot = Instantiate(_hotbarSlotPrefab, hotbarSlotsParent.transform);
                thisSlot.name = "HotbarSlot" + (i + 1);
                _hotbarSlots.Add(thisSlot);
            }

            Transform quantityObj = thisSlot.transform.Find("Quantity");
            Transform imageObj = thisSlot.transform.Find("Image");
            TextMeshProUGUI quantityTxt = quantityObj.GetComponent<TextMeshProUGUI>();
            Image image = imageObj.GetComponent<Image>();

            quantityTxt.text = "x" + quantity;
            if (quantity > 1)
            {
                quantityTxt.enabled = true;
            }
            else
            {
                quantityTxt.enabled = false;
            }

            image.sprite = sprite;
            image.enabled = true;

            // Set up the hover info
            Hover2DTooltip hoverTooltip = thisSlot.GetComponent<Hover2DTooltip>();
            hoverTooltip.infoLeft = itemName + "\n" + desc;
            hoverTooltip.infoRight = itemType;
        }
    }

    // DEBUG
    public void AddApple()
    {
        AddItem("apple", 1);
    }

    public void AddItem(string itemCodeName, int quantity)
    {
        _inventoryBackendManager.AddItem(itemCodeName, quantity);
        UpdateInventory();
    }

    public void RemoveItem(string itemCodeName, int quantity)
    {
        _inventoryBackendManager.RemoveItem(itemCodeName, quantity);
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        List<InventoryItem> inventory = _inventoryBackendManager.Inventory;
        while (_slots.Count > inventory.Count)
        {
            Destroy(_slots[_slots.Count - 1]);
            _slots.RemoveAt(_slots.Count - 1);
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            InventoryItem ii = inventory[i];

            Item item = ii.Item;
            int quantity = ii.Quantity;

            // Item properties
            string itemName = item.ItemName;
            string desc = item.Description;
            string itemType = item.ItemType.ToString();
            Sprite sprite = item.Sprite;

            GameObject thisSlot;
            if (i < _slots.Count)
            {
                thisSlot = _slots[i];
            }
            else
            {
                thisSlot = Instantiate(_slotPrefab, inventorySlotsParent.transform);
                thisSlot.name = "InventorySlot" + (i + 1);
                _slots.Add(thisSlot);
            }

            Transform quantityObj = thisSlot.transform.Find("Quantity");
            Transform imageObj = thisSlot.transform.Find("Image");
            TextMeshProUGUI quantityTxt = quantityObj.GetComponent<TextMeshProUGUI>();
            Image image = imageObj.GetComponent<Image>();

            quantityTxt.text = "x" + quantity;
            if (quantity > 1)
            {
                quantityTxt.enabled = true;
            }
            else
            {
                quantityTxt.enabled = false;
            }

            image.sprite = sprite;
            image.enabled = true;

            // Set up the hover info
            Hover2DTooltip hoverTooltip = thisSlot.GetComponent<Hover2DTooltip>();
            hoverTooltip.infoLeft = itemName + "\n" + desc;
            hoverTooltip.infoRight = itemType;
            hoverTooltip.enableTooltip();
        }
    }
}

public static class InventoryFetcher
{
    public static InventoryManager Manager = null;
}

public class InventoryBackendManager
{
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public int Capacity = int.MaxValue;

    public bool AddItem(string itemCodeName, int quantity)
    {
        (string, string, ItemTypeEnum, int, Sprite) itemData = ItemManager.GetItemData(itemCodeName);
        Item item = new Item(itemData.Item1, itemData.Item2, itemData.Item3, itemData.Item4, itemData.Item5);
        return AddItem(item, quantity);
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        if (Inventory.Count >= Capacity)
        {
            return false;
        }

        foreach (InventoryItem invItem in Inventory)
        {
            if (invItem.Item.Equals(item) && invItem.Quantity < invItem.Item.MaxStack)
            {
                Debug.Log("same item, stacking");
                invItem.Quantity += quantity;
                if (invItem.Quantity > item.MaxStack)
                {
                    int newStackAmount = invItem.Quantity - item.MaxStack;
                    invItem.Quantity = item.MaxStack;
                    InventoryItem newStack = new InventoryItem(item, newStackAmount);
                    Inventory.Add(newStack);
                }
                return true;
            }
        }

        InventoryItem ii = new InventoryItem(item, quantity);
        Inventory.Add(ii);
        return true;
    }

    public void RemoveItem(string itemCodeName, int quantity)
    {
        (string, string, ItemTypeEnum, int, Sprite) itemData = ItemManager.GetItemData(itemCodeName);
        Item item = new Item(itemData.Item1, itemData.Item2, itemData.Item3, itemData.Item4, itemData.Item5);
        RemoveItem(item, quantity);
    }

    public void RemoveItem(Item item, int quantity = 1)
    {
        for (int i = Inventory.Count - 1; i >= 0; i--)
        {
            InventoryItem invItem = Inventory[i];
            if (invItem.Item.Equals(item))
            {
                invItem.Quantity -= quantity;
                if (invItem.Quantity <= 0)
                {
                    Inventory.Remove(invItem);
                }
            }
        }
    }
}

public class InventoryItem
{
    public Item Item { get; private set; }
    public int Quantity;

    public InventoryItem(Item item, int quantity = 1)
    {
        this.Item = item;
        this.Quantity = quantity;
    }

    public InventoryItem(string itemCodeName, int quantity = 1)
    {
        this.Item = new Item(itemCodeName);
        this.Quantity = quantity;
    }
}

public class Item
{
    public String ItemName { get; private set; }
    public String Description { get; private set; }
    public ItemTypeEnum ItemType { get; private set; }
    public int MaxStack { get; private set; }
    public Sprite Sprite { get; private set; }

    // only reason default for sprite is null is because of the warning
    public Item(String name, String desc, ItemTypeEnum type, int maxStack = 64, Sprite sprite = null)
    {
        ItemName = name;
        Description = desc;
        ItemType = type;
        this.MaxStack = maxStack;
        this.Sprite = sprite;
    }

    public Item(String itemCodeName)
    {
        (string, string, ItemTypeEnum, int, Sprite) itemData = ItemManager.GetItemData(itemCodeName);
        
        ItemName = itemData.Item1;
        Description = itemData.Item2;
        ItemType = itemData.Item3;
        this.MaxStack = itemData.Item4;
        this.Sprite = itemData.Item5;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Item other = (Item)obj;

        // Only requires the same item name. (If future items share names, can cause issues)
        if (ItemName == other.ItemName) return true;

        return false;
    }
}