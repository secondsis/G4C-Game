using System;
using Main.Scripts;
using UnityEngine;

public class ShopFrontendManager : MonoBehaviour
{
    private static GameObject ShopUI;

    private void Awake()
    {
        ShopUI = transform.Find("UI").gameObject;
    }

    public static void OpenShop()
    {
        ShopUI.SetActive(true);
    }

    public static void CloseShop()
    {
        ShopUI.SetActive(false);
        DialogueUI.LaunchDialogue("SeedShopLeave1_0");
    }
}
