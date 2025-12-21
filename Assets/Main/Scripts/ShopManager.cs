using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopItem> stock;

    [SerializeField] private Transform _itemParent;
    private GameObject _shopItemPrefab;
    
    // Initialization Awake
    private List<GameObject> _shopItems = new List<GameObject>();

    private void Awake()
    {
        _shopItemPrefab = Resources.Load<GameObject>("Prefabs/ShopItem");
        foreach (ShopItem item in stock)
        {
            GameObject newObj = Instantiate(_shopItemPrefab, _itemParent);
            _shopItems.Add(newObj);
            
            string itemID = item.itemID;
            int cost = item.cost;

            (string, string, ItemTypeEnum, int, Sprite) itemData = ItemManager.GetItemData(itemID);
            
            // Initialize newObj's image, tooltip description, and cost text
            var hoverTooltip = newObj.GetComponent<Hover2DTooltip>();
            hoverTooltip.infoLeft = itemData.Item1.ToUpper() + "\n" + itemData.Item2;
            hoverTooltip.infoRight = "COST: $" + cost;
            
            newObj.transform.Find("Image").GetComponent<Image>().sprite = itemData.Item5;
            newObj.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "$" + cost;
        }
    }
    
    
}

[System.Serializable]
public class ShopItem
{
    public string itemID;
    public int cost;
}
