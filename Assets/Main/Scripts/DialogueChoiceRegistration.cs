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
        DialogueChoiceDictionary.Register("Kevin", Kevin);
        DialogueChoiceDictionary.Register("GotFactory", GotFactory);
        DialogueChoiceDictionary.Register("GotTrucks", GotTrucks);
        DialogueChoiceDictionary.Register("GotPigs", GotPigs);
        DialogueChoiceDictionary.Register("GotChairs", GotChairs);
        DialogueChoiceDictionary.Register("GotFarm", GotFarm);
        DialogueChoiceDictionary.Register("GotDirtHoles", GotDirtHoles);
        DialogueChoiceDictionary.Register("GotChurch", GotChurch);
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
        DialogueUI.LaunchDialogue("JoeWhosKevin");
    }
    private void GotFactory()
    {
        Dictionaries.WiseJoeTalks["Factory"] = true;
        DialogueUI.LaunchDialogue("JoeHatesPollution");
    }
    
    private void GotTrucks()
    {
        Dictionaries.WiseJoeTalks["Trucks"] = true;
        DialogueUI.LaunchDialogue("JoeWhatTrucks");
    }
    
    private void GotChurch()
    {
        Dictionaries.WiseJoeTalks["Church"] = true;
        DialogueUI.LaunchDialogue("JoeChurch");
    }
    
    // Farm is finished when player has $1000
    private void GotFarm()
    {
        Dictionaries.WiseJoeTalks["Farm"] = true;
        DialogueUI.LaunchDialogue("JoeIFinished");
    }
    
    private void GotDirtHoles()
    {
        Dictionaries.WiseJoeTalks["DirtHoles"] = true;
        DialogueUI.LaunchDialogue("JoeWhyDirt");
    }
    
    private void GotPigs()
    {
        Dictionaries.WiseJoeTalks["Pigs"] = true;
        DialogueUI.LaunchDialogue("JoeWhyPigs");
    }
    
    private void GotChairs()
    {
        Dictionaries.WiseJoeTalks["Chairs"] = true;
        DialogueUI.LaunchDialogue("JoeWhatTheChair");
    }
}
