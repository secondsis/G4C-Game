using UnityEngine;
using System.Collections.Generic;
using System;

public static class ItemManager
{
    private static readonly Dictionary<string, ItemData> Items;

    static ItemManager()
    {
        Debug.Log("Item Manager started");
        TextAsset json = Resources.Load<TextAsset>("Items");
        ItemDatabase db = JsonUtility.FromJson<ItemDatabase>(json.text);

        Items = new Dictionary<string, ItemData>();
        foreach (ItemEntry entry in db.items)
        {
            Items.Add(entry.id, entry.itemData);
        }
    }   

    private static Sprite GetItemIcon(string icon)
    {
        Sprite spr = Resources.Load<Sprite>("Icons/" + icon);
        if(!spr)
        {
            spr = Resources.Load<Sprite>("Icons/debug");
        }
        return spr;
    }

    /*
     * Returns the Name, Description, MaxStack, Icon
     */
    public static (string, string, ItemTypeEnum, int, Sprite) GetItemData(string itemCodeName)
    {
        ItemData data = Items[itemCodeName];
        string input = data.itemType;
        ItemTypeEnum itemType = ItemTypeEnum.OTHER;
        if (Enum.TryParse(input, out ItemTypeEnum type))
        {
            itemType = type;
        }
        else
        {
            Debug.LogWarning("Invalid enum string");
        }

        return (data.name, data.description, itemType, data.maxStack, GetItemIcon(data.icon));
    }

}
