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
    private void RegisterGameEvent(string dictionaryName, string dialogueName, string choiceTitle, string choiceAction)
    {
        Dictionaries.WiseJoeTalks[dictionaryName] = true;
        bool listenedAll = true;
        // Check if all dialogues have been listened to once
        foreach (bool entry in Dictionaries.WiseJoeTalks.Values)
        {
            if (!entry)
            {
                listenedAll = false;
                DebugScript.BetterDebug("Did not listen to all events.");
                break;
            }
        }

        if (listenedAll)
        {
            // ending scene is activatable
            // After this talk with Joe, the screen will fade out and outro
            // Invoke ending
            DebugScript.BetterDebug("Listened to ALL events!");
            EndingEnterer.Instance.StartEnding();
        }
        
        DialogueUI.LaunchDialogue(dialogueName);
        JoeInstance.Instance.RemoveGeneralChoice(choiceTitle, choiceAction);
    }

    private void GotEmpty()
    {
        RegisterGameEvent("Empty", "JoeWhyEmptyTown", "Why empty?", "GotEmpty");
    }

    private void Kevin()
    {
        RegisterGameEvent("Kevin", "JoeWhosKevin", "Kevin?", "Kevin");
    }
    private void GotFactory()
    {
        RegisterGameEvent("Factory", "JoeHatesPollution", "Factory?", "GotFactory");
    }
    
    private void GotTrucks()
    {
        RegisterGameEvent("Trucks", "JoeWhatTrucks", "Trucks?", "GotTrucks");
    }
    
    private void GotChurch()
    {
        RegisterGameEvent("Church", "JoeChurch", "Church?", "GotChurch");
    }
    
    // Farm is finished when player has $100
    private void GotFarm()
    {
        RegisterGameEvent("Farm", "JoeIFinished", "Farm?", "GotFarm");
    }
    
    private void GotDirtHoles()
    {
        RegisterGameEvent("DirtHoles", "JoeWhyDirt", "Dirt?", "GotDirtHoles");
    }
    
    private void GotPigs()
    {
        RegisterGameEvent("Pigs", "JoeWhyPigs", "Pigs?", "GotPigs");
    }
    
    private void GotChairs()
    {
        RegisterGameEvent("Chairs", "JoeWhatTheChair", "Chairs?", "GotChairs");
    }
}
