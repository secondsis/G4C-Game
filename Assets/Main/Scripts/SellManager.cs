using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellManager : MonoBehaviour
{
    [SerializeField] private GameObject sellMenuSlotPrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private Transform itemPanel;

    private GameObject _ui;
    
    private void Awake()
    {
        _ui = gameObject.transform.parent.Find("UI").gameObject;
        _ui.SetActive(false);
    }

    public void OpenSellMenu()
    {
        PopulateMenu();
        _ui.SetActive(true);
        
    }

    public void CloseSellMenu()
    {
        _ui.SetActive(false);
    }

    public void PopulateMenu()
    {
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

            quantityTxt.text = quantity.ToString();
            image.sprite = sprite;
            
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

                    img.sprite = sprite;
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
