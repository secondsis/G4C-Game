using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellManager : MonoBehaviour
{
    [SerializeField] private GameObject sellMenuSlotPrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private Transform itemPanel;
    
    private void Awake()
    {
        
    }

    public void OpenSellMenu()
    {
        
    }

    public void PopulateMenu()
    {
        List<InventoryItem> inventory = InventoryManager.Instance.GetInventory();
        // Only can sell items that are PRODUCT
        // Any others must be blacked/grayed out
        foreach (InventoryItem inventoryItem in inventory)
        {
            GameObject slot = Instantiate(sellMenuSlotPrefab, content.transform);
            if (inventoryItem.Item.ItemType == ItemTypeEnum.PRODUCT)
            {
                // Set the button onClick
                Button btn =  slot.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    // Open up itemPanel
                    Image img = itemPanel.Find("IMAGE").GetComponent<Image>();
                    TextMeshProUGUI nameTxt = itemPanel.Find("NameBG").Find("NAME").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI descTxt = itemPanel.transform.Find("DescBG").Find("DESCRIPTION").GetComponent<TextMeshProUGUI>();

                    img.sprite = inventoryItem.Item.Sprite;
                    img.enabled = true;
                    
                    nameTxt.text = inventoryItem.Item.ItemName;
                    nameTxt.enabled = true;
                    
                    descTxt.text = inventoryItem.Item.Description;
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
}
