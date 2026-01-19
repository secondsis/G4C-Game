using Main.Scripts;
using UnityEngine;

public class DialogueChoiceRegistration : MonoBehaviour
{
    private void Awake()
    {
        DialogueChoiceDictionary.Register("Default", Default);
        DialogueChoiceDictionary.Register("SeedShopNo", SeedShopNo);
        DialogueChoiceDictionary.Register("OpenSeedShop", OpenSeedShop);
    }

    private void Default()
    {
        Debug.Log("Choice option clicked!");
    }

    private void SeedShopNo()
    {
        DialogueUI.LaunchDialogue("SeedShopNo");
    }
    

    private void OpenSeedShop()
    {
        ShopFrontendManager.OpenShop();
    }
}
