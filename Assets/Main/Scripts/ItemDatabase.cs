using System.Collections.Generic;

// This is formatted based on the JSON.
[System.Serializable]
public class ItemDatabase
{
    public List<ItemEntry> items;
}

[System.Serializable]
public class ItemEntry
{
    public string id;
    public ItemData itemData;
}

[System.Serializable]
public class ItemData
{
    public string name;
    public string description;
    public string itemType;
    public int maxStack;
    public string icon;  // name of a sprite in Resources
}
