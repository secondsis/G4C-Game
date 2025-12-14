using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private InventoryBackendManager inventoryBackendManager = new InventoryBackendManager();
    [SerializeField] private GameObject inventorySlotsParent;
    private List<GameObject> slots = new List<GameObject>();
    private GameObject slotPrefab;

    private void Awake()
    {
        slotPrefab = Resources.Load<GameObject>("Prefabs/InventorySlot");
    }

    public void AddItem(string itemCodeName, int quantity)
    {
        (string, string, ItemTypeEnum, int, Sprite) itemData = ItemManager.GetItemData(itemCodeName);
        Item item = new Item(itemData.Item1, itemData.Item2, itemData.Item3, itemData.Item4, itemData.Item5);
        inventoryBackendManager.addItem(item, quantity);
        UpdateInventory();
    }

    public void RemoveItem(string itemCodeName, int quantity)
    {
        (string, string, ItemTypeEnum, int, Sprite) itemData = ItemManager.GetItemData(itemCodeName);
        Item item = new Item(itemData.Item1, itemData.Item2, itemData.Item3, itemData.Item4, itemData.Item5);
        inventoryBackendManager.RemoveItem(item, quantity);
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        List<InventoryItem> inventory = inventoryBackendManager.inventory;
        while (slots.Count > inventory.Count)
        {
            Destroy(slots[slots.Count - 1]);
            slots.RemoveAt(slots.Count - 1);
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            InventoryItem ii = inventory[i];

            Item item = ii.item;
            int quantity = ii.quantity;

            // Item properties
            string itemName = item.itemName;
            string desc = item.description;
            string itemType = item.itemType.ToString();
            // Figure out stacking with maxStack and slot management
            int maxStack = item.maxStack;
            Sprite sprite = item.sprite;

            GameObject thisSlot;
            if (i < slots.Count)
            {
                thisSlot = slots[i];
            }
            else
            {
                thisSlot = Instantiate(slotPrefab, inventorySlotsParent.transform);
                slots.Add(thisSlot);
            }

            Transform quantityObj = thisSlot.transform.Find("Quantity");
            Transform imageObj = thisSlot.transform.Find("Image");
            TextMeshPro quantityTxt = quantityObj.GetComponent<TextMeshPro>();
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

            // Set up the hover info
            Hover2DTooltip hoverTooltip = thisSlot.GetComponent<Hover2DTooltip>();
            hoverTooltip.enabled = true;
            hoverTooltip.infoLeft = itemName + "\n" + desc;
            hoverTooltip.infoRight = itemType;
        }
    }
}

public class InventoryBackendManager
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public int capacity = int.MaxValue;

    public bool addItem(Item item, int quantity = 1)
    {
        if (inventory.Count >= capacity)
        {
            return false;
        }

        foreach (InventoryItem invItem in inventory)
        {
            if (invItem.item.Equals(item))
            {
                invItem.quantity += quantity;
                return true;
            }
        }

        InventoryItem ii = new InventoryItem(item, quantity);
        inventory.Add(ii);
        return true;
    }

    public void RemoveItem(Item item, int quantity = 1)
    {
        foreach (InventoryItem invItem in inventory)
        {
            if (invItem.item.Equals(item))
            {
                invItem.quantity -= quantity;
                if (invItem.quantity <= 0)
                {
                    inventory.Remove(invItem);
                }
            }
        }
    }
}

public class InventoryItem
{
    public Item item { get; private set; }
    public int quantity;

    public InventoryItem(Item item, int quantity = 1)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

public class Item
{
    public String itemName { get; private set; }
    public String description { get; private set; }
    public ItemTypeEnum itemType { get; private set; }
    public int maxStack { get; private set; }
    public Sprite sprite { get; private set; }

    // only reason default for sprite is null is because of the warning
    public Item(String name, String desc, ItemTypeEnum type, int maxStack = 64, Sprite sprite = null)
    {
        itemName = name;
        description = desc;
        itemType = type;
        this.maxStack = maxStack;
        this.sprite = sprite;
    }
}