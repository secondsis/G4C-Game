using System.Collections.Generic;

// This is formatted based on the JSON.
[System.Serializable]
public class ItemDatabase
{
    public Dictionary<string, ItemData> items;
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
