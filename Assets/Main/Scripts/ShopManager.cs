using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopItem> stock;

    [SerializeField] private Transform _itemParent;
    private GameObject _shopItemPrefab;
    private PlayerStatManager _playerStatManager;
    
    // // Initialization Awake
    // private List<GameObject> _shopItems = new List<GameObject>();

    private void Awake()
    {
        _shopItemPrefab = Resources.Load<GameObject>("Prefabs/ShopItem");
        _playerStatManager = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerManagers")
            .GetComponent<PlayerStatManager>();
        foreach (ShopItem item in stock)
        {
            GameObject newObj = Instantiate(_shopItemPrefab, _itemParent);
            // _shopItems.Add(newObj);
            
            string itemID = item.itemID;
            int cost = item.cost;

            (string, string, ItemTypeEnum, int, Sprite) itemData = ItemManager.GetItemData(itemID);
            
            // Initialize newObj's image, tooltip description, and cost text
            var hoverTooltip = newObj.GetComponent<Hover2DTooltip>();
            hoverTooltip.infoLeft = itemData.Item1.ToUpper() + "\n" + itemData.Item2;
            hoverTooltip.infoRight = "COST: $" + cost;
            hoverTooltip.enableTooltip();
            
            var shopItemButton =  newObj.GetComponent<Button>();
            shopItemButton.onClick.AddListener(() => BuyItem(itemID, cost));
            
            var image = newObj.transform.Find("Image").GetComponent<Image>();
            image.sprite = itemData.Item5;
            image.enabled = true;

            var costText = newObj.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = "$" + cost;
            costText.enabled = true;

        }
    }

    public void BuyItem(string itemID, int cost)
    {
        // Take Money from Player's Wallet
        if (!_playerStatManager.SpendMoney(cost)) return;
        
        InventoryManager.Instance.AddItem(itemID, 1);
    }
}

[System.Serializable]
public class ShopItem
{
    public string itemID;
    public int cost;
}
