using System;
using System.Collections.Generic;
using Main.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // BUG: The equip/unequip AND itemPanel does not appear properly when dealing with multiple items
    public static InventoryManager Instance;
    
    // Main Inventory
    private readonly InventoryBackendManager _inventoryBackendManager = new InventoryBackendManager();
    [SerializeField] private GameObject inventorySlotsParent;
    private readonly List<GameObject> _slots = new List<GameObject>();
    private GameObject _slotPrefab;
    private bool _inventoryOpened = false;

    // Hotbar (should contain the indices of the inventory)
    private readonly List<int> _hotbar = new List<int>(10);
    [SerializeField] private GameObject hotbarSlotsParent; // change this to be accuratevvvggggg  gggggg bgggrrvvbfuuxdddd
    private readonly List<GameObject> _hotbarSlots = new List<GameObject>(10);
    private GameObject _hotbarSlotPrefab;
    public InventoryItem CurrentlyEquipped;
    
    // UI Side
    private GameObject _uiGameObject;
    
    // ItemPanel
    private int _itemPanelHashCode = -1; // Updated when opening _itemPanel
    private GameObject _itemPanel;
    private Image _itemPanelImage; 
    private TextMeshProUGUI _itemPanelName;
    private TextMeshProUGUI _itemPanelDescription;
    private Image _itemPanelButtonImage;
    private TextMeshProUGUI _itemPanelButtonText;

    private void Awake()
    {
        _slotPrefab = Resources.Load<GameObject>("Prefabs/InventorySlot");
        _hotbarSlotPrefab = Resources.Load<GameObject>("Prefabs/HotbarSlot");
        _uiGameObject = gameObject.transform.parent.gameObject.transform.Find("UI").gameObject;
        _itemPanel = _uiGameObject.transform.Find("ItemPanel").gameObject;
        _itemPanelImage = _itemPanel.transform.Find("IMAGE").GetComponent<Image>();
        _itemPanelName = _itemPanel.transform.Find("NameBG").Find("NAME").GetComponent<TextMeshProUGUI>();
        _itemPanelDescription = _itemPanel.transform.Find("DescBG").Find("DESCRIPTION").GetComponent<TextMeshProUGUI>();
        _itemPanelButtonImage = _itemPanel.transform.Find("EquipObject").GetComponent<Image>();
        _itemPanelButtonText = _itemPanel.transform.Find("EquipObject").Find("EQUIPTOGGLE").GetComponent<TextMeshProUGUI>();
        _itemPanel.SetActive(false);
        _uiGameObject.SetActive(false);
        _inventoryOpened = false;
        
        // THIS IS A SINGLETON
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ToggleEquipButton()
    {
        if (_itemPanelButtonImage.color.g.Equals(1f))
        {
            // EQUIP THE ITEM!
            _itemPanelButtonImage.color = new Color(1, 0, 0);
            _itemPanelButtonText.text = "Unequip";
            EquipItem(_itemPanelHashCode);
        }
        else
        {
            // UNEQUIP THE ITEM!
            _itemPanelButtonImage.color = new Color(0, 1, 0);
            _itemPanelButtonText.text = "Equip";
            UnequipItem(_itemPanelHashCode);
        }
    }
    
    private void EnableAllTooltips()
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
    
    private void DisableAllTooltips()
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

    public void ToggleInventory()
    {
        if (_inventoryOpened)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    private void OpenInventory()
    {
        gameObject.transform.parent.gameObject.transform.Find("UI").gameObject.SetActive(true);
        _inventoryOpened = true;
        // FUTURE: ADD ANIMATIONS TO OPENING
        
        EnableAllTooltips();
    }

    private void CloseInventory()
    {
        gameObject.transform.parent.gameObject.transform.Find("UI").gameObject.SetActive(false);
        _inventoryOpened = false;
        // FUTURE: ADD ANIMATIONS
        
        DisableAllTooltips();
    }

    // Should be called upon using the equip button
    public void EquipItem(int invItemHash)
    {
        _hotbar.Add(invItemHash);

        UpdateHotbar();
    }
    
    /** Unequip Item
     * Using the inventoryitem hashcode, it will find the item and remove it from the hotbar
     */
    public void UnequipItem(int invItemHash)
    {
        if (!_hotbar.Remove(invItemHash))
        {
            Debug.LogWarning("Couldn't remove that inventory hash.");
        }
        UpdateHotbar();
    }

    public void DecrementCurrentlyEquipped()
    {
        CurrentlyEquipped.Quantity--;
        if (CurrentlyEquipped.Quantity <= 0)
        {
            ToolEvents.InvokeToolUnequip();
            CurrentlyEquipped = null;
        }
    }

    // Need to rework hotbar to be a dynamic list that has a capacity cap. 
    public void UpdateHotbar()
    {
        while (_hotbarSlots.Count > _hotbar.Count)
        {
            Destroy(_hotbarSlots[_hotbarSlots.Count - 1]);
            _hotbarSlots.RemoveAt(_hotbarSlots.Count - 1);
        }

        for (int i = 0; i < _hotbar.Count; i++)
        {
            int iiHash = _hotbar[i];
            if (iiHash == -1)
            {
                Debug.Log("Reached end of hotbar");
                break;
            }
            InventoryItem ii = _inventoryBackendManager.Inventory.Find(item => item.GetHashCode() == iiHash);
            
            Item item = ii.Item;
            int quantity = ii.Quantity;

            // Item properties
            string itemName = item.ItemName;
            string desc = item.Description;
            string itemType = item.ItemType.ToString();
            Sprite sprite = item.Sprite;
            string prefabPath = item.PrefabPath;

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

                if (item.Equippable)
                {
                    GameObject itemPrefab = Resources.Load<GameObject>(prefabPath);
                    //ADD THE CLICK BUTTON FUNCTION
                    thisSlot.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        // This will equip the item, according to the prefab path
                        // Prefab Path should/must be a InventoryItem variable
                        // Also add ability to unequip (ALSO unequip when u remove the item from hotbar)
                        if (!ToolHandler.isToolEquipped(itemPrefab.name))
                        {
                            Debug.Log("Equipped");
                            ToolEvents.InvokeToolEquip(itemPrefab, ii);
                            CurrentlyEquipped = ii;
                            // Send an event that tells every script what the new item is
                        }
                        else
                        {
                            Debug.Log("Unequipped");
                            ToolEvents.InvokeToolUnequip();
                            CurrentlyEquipped = null;
                        }
                    
                    });
                }

            }

            Transform quantityObj = thisSlot.transform.Find("Quantity");
            Transform imageObj = thisSlot.transform.Find("Image");
            TextMeshProUGUI quantityTxt = quantityObj.GetComponent<TextMeshProUGUI>();
            Image image = imageObj.GetComponent<Image>();

            quantityTxt.text = "x" + quantity;
            quantityTxt.enabled = quantity > 1;

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
        RemoveHotbarItem(itemCodeName);
        
    }

    public void RemoveHotbarItem(string itemCodeName)
    {
        // BUG: This will not account for duplicate items.
        _hotbar.Remove(itemCodeName.GetHashCode());
        UpdateHotbar();
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
            
            // This is clicking the inventory slot to open/close the itemPanel
            thisSlot.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Clicked inventory slot");
                // Using HashCodes might be expensive
                int newHash = ii.GetHashCode();
                if (_itemPanel.activeSelf && _itemPanelHashCode == newHash)
                {
                    _itemPanel.SetActive(false);
                    return;
                }

                _itemPanelHashCode = newHash;
                _itemPanelImage.sprite = sprite;
                _itemPanelImage.enabled = true;
                _itemPanelName.text = itemName;
                _itemPanelDescription.text = desc;
                _itemPanelDescription.enabled = true;
                _itemPanel.SetActive(true);
            });
            
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

public class InventoryBackendManager
{
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public int Capacity = int.MaxValue;

    public bool AddItem(string itemCodeName, int quantity)
    {
        (string, string, ItemTypeEnum, int, Sprite, string) itemData = ItemManager.GetItemData(itemCodeName);
        Item item = new Item(itemData.Item1, itemData.Item2, itemData.Item3, itemData.Item4, itemData.Item5, itemData.Item6);
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
    
    // Returns false if inventoryItem quantity is gone
    public bool RemoveItem(string itemCodeName, int quantity)
    {
        (string, string, ItemTypeEnum, int, Sprite, string) itemData = ItemManager.GetItemData(itemCodeName);
        Item item = new Item(itemData.Item1, itemData.Item2, itemData.Item3, itemData.Item4, itemData.Item5, itemData.Item6);
        return RemoveItem(item, quantity);
    }

    // Returns false if inventoryItem quantity is gone
    public bool RemoveItem(Item item, int quantity = 1)
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
                    return false;
                }
            }
        }

        return true;
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
    public String ItemName { get; }
    public String Description { get; private set; }
    public ItemTypeEnum ItemType { get; private set; }
    public int MaxStack { get; private set; }
    public Sprite Sprite { get; private set; }
    public bool Equippable { get; private set; }
    public String PrefabPath { get; private set; }

    // only reason default for sprite is null is because of the warning
    public Item(String name, String desc, ItemTypeEnum type, int maxStack = 64, Sprite sprite = null, String prefabPath="")
    {
        ItemName = name;
        Description = desc;
        ItemType = type;
        this.MaxStack = maxStack;
        this.Sprite = sprite;
        Equippable = !string.IsNullOrEmpty(prefabPath);
        PrefabPath = prefabPath;
    }

    public Item(String itemCodeName)
    {
        // need to add prefabpath to the itemdata
        (string, string, ItemTypeEnum, int, Sprite, string) itemData = ItemManager.GetItemData(itemCodeName);
        
        ItemName = itemData.Item1;
        Description = itemData.Item2;
        ItemType = itemData.Item3;
        this.MaxStack = itemData.Item4;
        this.Sprite = itemData.Item5;
        PrefabPath = itemData.Item6;
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

    public override int GetHashCode()
    {
        // Must make ItemName immutable { get; }
        return ItemName.GetHashCode();
    }
}