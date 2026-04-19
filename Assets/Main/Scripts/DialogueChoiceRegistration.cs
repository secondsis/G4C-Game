using Main.Scripts;
using UnityEngine;

public class DialogueChoiceRegistration : MonoBehaviour
{
    private void Awake()
    {
        DialogueChoiceDictionary.Register("Default", Default);
        DialogueChoiceDictionary.Register("SeedShopNo", SeedShopNo);
        DialogueChoiceDictionary.Register("OpenSeedShop", OpenSeedShop);
        DialogueChoiceDictionary.Register("DemoSeedShopNo", DemoSeedShopNo);
        
    }

    private void Default()
    {
        Debug.Log("Choice option clicked!");
    }
    
    private void DemoSeedShopNo()
    {
        DialogueUI.LaunchDialogue("DemoSeedShopNo");
    }

    private void SeedShopNo()
    {
        DialogueUI.LaunchDialogue("SeedShopNo");
    }

    private void OpenSeedShop()
    {
        ShopFrontendManager.OpenShop("Kevin", "Selling seeds is a tradition my family always carried.");
    }
    
    // Gotta make functions for each "GameEvent"...

    private void Kevin()
    {
        Dictionaries.WiseJoeTalks["Kevin"] = true;
    }
    
    private void GotFactory()
    {
        Dictionaries.WiseJoeTalks["Factory"] = true;
    }
    
    private void GotTrucks()
    {
        Dictionaries.WiseJoeTalks["Trucks"] = true;
    }
    
    private void GotChurch()
    {
        Dictionaries.WiseJoeTalks["Church"] = true;
    }
    
    private void GotFarm()
    {
        Dictionaries.WiseJoeTalks["Farm"] = true;
    }
    
    private void GotDirtHoles()
    {
        Dictionaries.WiseJoeTalks["DirtHoles"] = true;
    }
    
    private void GotPigs()
    {
        Dictionaries.WiseJoeTalks["Pigs"] = true;
    }
    
    private void GotChairs()
    {
        Dictionaries.WiseJoeTalks["Chairs"] = true;
    }
}
