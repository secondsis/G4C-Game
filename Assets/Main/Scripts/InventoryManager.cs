using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private readonly InventoryBackendManager _inventoryBackendManager = new InventoryBackendManager();
    [SerializeField] private GameObject inventorySlotsParent;
    private readonly List<GameObject> _slots = new List<GameObject>();
    private GameObject _slotPrefab;
    [SerializeField] private GameObject hotbarSlotsParent;
    private readonly List<GameObject> _hotbarSlots = new List<GameObject>();
    private GameObject _hotbarSlotPrefab;

    private readonly InventoryItem[] _hotbar = new InventoryItem[10];
    private bool _inventoryOpened = false;
    
    // UI Side
    private GameObject _uiGameObject;
    
    // ItemPanel
    private GameObject _itemPanel;
    private Image _itemPanelImage; 
    private TextMeshProUGUI _itemPanelName;
    private TextMeshProUGUI _itemPanelDescription;
    private Image _itemPanelButtonImage;
    private Button _itemPanelButton;
    private TextMeshProUGUI _itemPanelButtonText;

    private void Awake()
    {
        _slotPrefab = Resources.Load<GameObject>("Prefabs/InventorySlot");
        _uiGameObject = gameObject.transform.parent.gameObject.transform.Find("UI").gameObject;
        _itemPanel = _uiGameObject.transform.Find("ItemPanel").gameObject;
        _itemPanelImage = _itemPanel.transform.Find("IMAGE").GetComponent<Image>();
        _itemPanelName = _itemPanel.transform.Find("NameBG").Find("NAME").GetComponent<TextMeshProUGUI>();
        _itemPanelDescription = _itemPanel.transform.Find("DescBG").Find("DESCRIPTION").GetComponent<TextMeshProUGUI>();
        _itemPanelButtonImage = _itemPanel.transform.Find("EquipObject").GetComponent<Image>();
        _itemPanelButton = _itemPanel.transform.Find("EquipObject").GetComponent<Button>();
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
            
        }
        else
        {
            // UNEQUIP THE ITEM!
            _itemPanelButtonImage.color = new Color(0, 1, 0);
            _itemPanelButtonText.text = "Equip";
            
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
    
    public void EquipItem(string itemCodeName, int quantity)
    {
        for (int i = 0; i < _hotbar.Length; i++)
        {
            if (_hotbar[i] == null)
            {
                _hotbar[i] = new InventoryItem(itemCodeName, quantity);
                break;
            }
        }
    }

    public void UnequipItem(string itemCodeName, int quantity)
    {
        for (int i = 0; i < _hotbar.Length; i++)
        {
            if (_hotbar[i].Equals(new InventoryItem(itemCodeName, quantity)))
            {
                _hotbar[i] = null;
                break;
            }
        }
    }

    public void UpdateHotbar()
    {
        while (_hotbarSlots.Count > _hotbar.Length)
        {
            Destroy(_hotbarSlots[_hotbarSlots.Count - 1]);
            _hotbarSlots.RemoveAt(_hotbarSlots.Count - 1);
        }

        for (int i = 0; i < _hotbar.Length; i++)
        {
            InventoryItem ii = _hotbar[i];

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

            thisSlot.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Clicked inventory slot");
                // KNOWN BUG:
                // If these are the same, don't change the itemPanel BUT MAKE SURE TO STILL UPDATE THE INDEX IF PLAYER
                // CLICKS 'EQUIP' (ex. x64 stack of apples vs x2 stack of apples)
                if (_itemPanel.activeSelf && _itemPanelName.text == itemName)
                {
                    _itemPanel.SetActive(false);
                    return;
                }
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
    public String ItemName { get; }
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

    public override int GetHashCode()
    {
        // Must make ItemName immutable { get; }
        return ItemName.GetHashCode();
    }
}