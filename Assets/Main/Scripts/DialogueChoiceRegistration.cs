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
        DialogueChoiceDictionary.Register("GotEmpty", GotEmpty);
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
        ShopFrontendManager.OpenShop("Shopkeeper", "I AM THE SEED GOD.");
    }
    
    // Gotta make functions for each "GameEvent"...

    private void GotEmpty()
    {
        Dictionaries.WiseJoeTalks["Empty"] = true;
        // Check if all dialogues have been listened to once
        
        DialogueUI.LaunchDialogue("JoeWhyEmptyTown");
        JoeInstance.Instance.RemoveGeneralChoice("Why empty?", "GotEmpty");
    }

    private void Kevin()
    {
        Dictionaries.WiseJoeTalks["Kevin"] = true;
        // Check if all dialogues have been listened to once
        
        DialogueUI.LaunchDialogue("JoeWhosKevin");
        JoeInstance.Instance.RemoveGeneralChoice("Kevin?", "Kevin");
    }
    private void GotFactory()
    {
        Dictionaries.WiseJoeTalks["Factory"] = true;
        DialogueUI.LaunchDialogue("JoeHatesPollution");
        JoeInstance.Instance.RemoveGeneralChoice("Factory?", "GotFactory");
    }
    
    private void GotTrucks()
    {
        Dictionaries.WiseJoeTalks["Trucks"] = true;
        DialogueUI.LaunchDialogue("JoeWhatTrucks");
        JoeInstance.Instance.RemoveGeneralChoice("Trucks?", "GotTrucks");
    }
    
    private void GotChurch()
    {
        Dictionaries.WiseJoeTalks["Church"] = true;
        DialogueUI.LaunchDialogue("JoeChurch");
        JoeInstance.Instance.RemoveGeneralChoice("Church?", "GotChurch");
    }
    
    // Farm is finished when player has $100
    private void GotFarm()
    {
        Dictionaries.WiseJoeTalks["Farm"] = true;
        DialogueUI.LaunchDialogue("JoeIFinished");
        JoeInstance.Instance.RemoveGeneralChoice("Farm?", "GotFarm");
    }
    
    private void GotDirtHoles()
    {
        Dictionaries.WiseJoeTalks["DirtHoles"] = true;
        DialogueUI.LaunchDialogue("JoeWhyDirt");
        JoeInstance.Instance.RemoveGeneralChoice("Dirt?", "GotDirtHoles");
    }
    
    private void GotPigs()
    {
        Dictionaries.WiseJoeTalks["Pigs"] = true;
        DialogueUI.LaunchDialogue("JoeWhyPigs");
        JoeInstance.Instance.RemoveGeneralChoice("Pigs?", "GotPigs");
    }
    
    private void GotChairs()
    {
        Dictionaries.WiseJoeTalks["Chairs"] = true;
        DialogueUI.LaunchDialogue("JoeWhatTheChair");
        JoeInstance.Instance.RemoveGeneralChoice("Chairs?", "GotChairs");
    }
}
