using System;
using System.Collections.Generic;
using Main.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    
    // EVENTS
    public static event Action OnInventoryOpen;
    public static event Action OnInventoryClose;
    
    // Main Inventory
    private readonly InventoryBackendManager _inventoryBackendManager = new InventoryBackendManager();
    [SerializeField] private GameObject inventorySlotsParent;
    private readonly List<GameObject> _slots = new List<GameObject>();
    private GameObject _slotPrefab;
    private bool _inventoryOpened = false;

    // Hotbar (should contain the indices of the inventory)
    private readonly List<int> _hotbar = new List<int>(10);
    [SerializeField] private GameObject hotbarSlotsParent; // change this to be accurate
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
    
    // Blocking Menus
    private bool _shopBlocking = false;
    private bool _sellMenuBlocking = false;

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
    
    private void SetSellMenuBlockingTrue()
    {
        _sellMenuBlocking = true;
    }
    private void SetSellMenuBlockingFalse()
    {
        _sellMenuBlocking = false;
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
        SellManager.OnSellMenuOpen += SetSellMenuBlockingTrue;
        SellManager.OnSellMenuClose += SetSellMenuBlockingFalse;

        ShopFrontendManager.OnShopOpen += SetShopBlockingTrue;
        ShopFrontendManager.OnShopClose += SetShopBlockingFalse;
    }

    private void OnDisable()
    {
        SellManager.OnSellMenuOpen -= SetSellMenuBlockingTrue;
        SellManager.OnSellMenuClose -= SetSellMenuBlockingFalse;

        ShopFrontendManager.OnShopOpen -= SetShopBlockingTrue;
        ShopFrontendManager.OnShopClose -= SetShopBlockingFalse;
    }

    public List<InventoryItem> GetInventory()
    {
        return _inventoryBackendManager.Inventory;
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
        if (_shopBlocking || _sellMenuBlocking)
        {
            DebugScript.BetterDebug("WARNING: Cannot open inventory because shop or sell menu are open");
            return;
        }
        gameObject.transform.parent.gameObject.transform.Find("UI").gameObject.SetActive(true);
        _inventoryOpened = true;
        // TODO: ADD ANIMATIONS TO OPENING
        
        EnableAllTooltips();
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
        OnInventoryOpen?.Invoke();
    }

    private void CloseInventory()
    {
        gameObject.transform.parent.gameObject.transform.Find("UI").gameObject.SetActive(false);
        _inventoryOpened = false;
        // TODO: ADD ANIMATIONS
        
        DisableAllTooltips();
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        OnInventoryClose?.Invoke();
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
        if (CurrentlyEquipped == null) return;
        RemoveItem(CurrentlyEquipped, 1);
        if (CurrentlyEquipped.Quantity <= 0)
        {
            Events.InvokeToolUnequip();
            CurrentlyEquipped = null;
        }
    }

    private void ResetItemPanel()
    {
        _itemPanel.SetActive(false);
        Instance._itemPanelHashCode = -1;
        Instance._itemPanelImage.enabled = false;
        Instance._itemPanelName.text = "";
        Instance._itemPanelDescription.text = "";
        Instance._itemPanelDescription.enabled = false;
    
        Instance._itemPanelButtonImage.color = new Color(0, 1, 0);
        Instance._itemPanelButtonText.text = "Equip";

    }
    
    public void UpdateHotbar()
    {
        Debug.Log("Updating hotbar");
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

                
                
            }
            // MUST REMOVE ALL LISTENERS FIRST!! Don't want "artifacts" from old stuff
            Button thisSlotBtn = thisSlot.GetComponent<Button>();
            thisSlotBtn.onClick.RemoveAllListeners();
            if (item.Equippable)
            {
                GameObject itemPrefab = Resources.Load<GameObject>(prefabPath);
                //ADD THE CLICK BUTTON FUNCTION
                thisSlotBtn.onClick.AddListener(() =>
                {
                    // This will equip the item, according to the prefab path
                    // Prefab Path should/must be a InventoryItem variable
                    // Also add ability to unequip (ALSO unequip when u remove the item from hotbar)
                    if (CurrentlyEquipped != ii)
                    {
                        Debug.Log("Equipped " + ii.Item.ItemName);
                        Events.InvokeToolEquip(itemPrefab);
                        // 
                        CurrentlyEquipped = ii;
                        // Send an event that tells every script what the new item is
                    }
                    else
                    {
                        Debug.Log("Unequipped");
                        Events.InvokeToolUnequip();
                        CurrentlyEquipped = null;
                    }
                    
                });
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
        UpdateHotbar();
    }

    public void RemoveItem(InventoryItem ii, int quantityToRemove)
    {
        // Also remember to update the inventory/hotbar quantity text
        bool isItemRemaining = _inventoryBackendManager.RemoveItem(ii, quantityToRemove);
        if (!isItemRemaining)
        {
            RemoveHotbarItem(ii);
            ResetItemPanel();
        }
        
        UpdateInventory();
        UpdateHotbar();
    }

    public InventoryItem GetInventoryItemFromHashCode(int hashCode)
    {
        foreach(InventoryItem ii in _inventoryBackendManager.Inventory)
        {
            if (ii.GetHashCode() == hashCode) return ii;
        }

        return null;
    }
    
    public void RemoveHotbarItem(InventoryItem ii)
    {
        _hotbar.Remove(ii.GetHashCode());
    }

    public void UpdateInventory(int earliestIndexRemoved=-1)
    {
        // TODO: make this function more efficient, utilizing the indices that were removed/modified
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
                // Updating listeners now
                thisSlot.GetComponent<Button>().onClick.RemoveAllListeners();
                thisSlot.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // Using HashCodes might be expensive
                    int newHash = ii.GetHashCode();
                    Debug.Log("This hash: " + newHash);
                    if (Instance._itemPanel.activeSelf && Instance._itemPanelHashCode == newHash)
                    {
                        Instance._itemPanel.SetActive(false);
                        return;
                    }

                    Instance._itemPanelHashCode = newHash;
                    Instance._itemPanelImage.sprite = sprite;
                    Instance._itemPanelImage.enabled = true;
                    Instance._itemPanelName.text = itemName;
                    Instance._itemPanelDescription.text = desc;
                    Instance._itemPanelDescription.enabled = true;
                    Instance._itemPanel.SetActive(true);
                    // Reset the itemPanel equip/unequip button (but check if item is in hotbar already)
                    if (_hotbar.Contains(ii.GetHashCode()))
                    {
                        Instance._itemPanelButtonImage.color = new Color(1, 0, 0);
                        Instance._itemPanelButtonText.text = "Unequip";
                    }
                    else
                    {
                        Instance._itemPanelButtonImage.color = new Color(0, 1, 0);
                        Instance._itemPanelButtonText.text = "Equip";
                    }

                });
            }
            else
            {
                thisSlot = Instantiate(_slotPrefab, inventorySlotsParent.transform);
                thisSlot.name = "InventorySlot" + (i + 1);
                _slots.Add(thisSlot);
                // Why does the hashcode not update when the inventory updates?
                // I press slot 1, but it interprets as the old slot 1 (contains carrot seeds instead of carrot)
                // It is locked to an old InventoryItem, and the function is never updated
                thisSlot.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // Using HashCodes might be expensive
                    int newHash = ii.GetHashCode();
                    Debug.Log("This hash: " + newHash);
                    if (Instance._itemPanel.activeSelf && Instance._itemPanelHashCode == newHash)
                    {
                        Instance._itemPanel.SetActive(false);
                        return;
                    }
                
                    Instance._itemPanelHashCode = newHash;
                    Instance._itemPanelImage.sprite = sprite;
                    Instance._itemPanelImage.enabled = true;
                    Instance._itemPanelName.text = itemName;
                    Instance._itemPanelDescription.text = desc;
                    Instance._itemPanelDescription.enabled = true;
                    Instance._itemPanel.SetActive(true);
                    // Reset the itemPanel equip/unequip button (but check if item is in hotbar already)
                    if (_hotbar.Contains(ii.GetHashCode()))
                    {
                        Instance._itemPanelButtonImage.color = new Color(1, 0, 0);
                        Instance._itemPanelButtonText.text = "Unequip";
                    }
                    else
                    {
                        Instance._itemPanelButtonImage.color = new Color(0, 1, 0);
                        Instance._itemPanelButtonText.text = "Equip";
                    }
                    
                });
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

public class InventoryBackendManager
{
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public int Capacity = int.MaxValue;

    public InventoryItem GetInventoryItem(int invItemHash)
    {
        foreach (InventoryItem invItem in Inventory)
        {
            if (invItem.GetHashCode() == invItemHash)
            {
                return invItem;
            }
        }

        return null;
    }

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
    
    public bool RemoveItem(InventoryItem ii, int quantityToRemove=1)
    {
        ii.Quantity -= quantityToRemove;
        if (ii.Quantity <= 0)
        {
            Inventory.Remove(ii);
            return false;
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