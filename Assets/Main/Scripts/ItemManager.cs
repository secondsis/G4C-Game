using UnityEngine;
using System.Collections.Generic;
using System;

public class ItemManager
{
    private static Dictionary<string, ItemData> Items;

    static ItemManager()
    {
        Debug.Log("Item Manager started");
        TextAsset json = Resources.Load<TextAsset>("Items");
        ItemDatabase db = JsonUtility.FromJson<ItemDatabase>(json.text);

        Items = db.items;
    }

    private static Sprite GetItemIcon(string id)
    {
        var data = Items[id];
        Sprite spr = Resources.Load<Sprite>("Icons/" + data.icon);
        if(spr == null)
        {
            spr = Resources.Load<Sprite>("Icons/debug");
        }
        return spr;
    }

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
