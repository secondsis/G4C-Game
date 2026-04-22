using System;
using Main.Scripts;
using UnityEngine;

public class WiseKevinSummoner : MonoBehaviour
{
    [SerializeField] private GameObject WiseKevin;

    private bool inKevinDialogue = false;
    
    private void Start()
    {
        DialogueUI.Instance.OnDialogueUIActiveChanged += CheckForKevin;
    }

    private void CheckForKevin(bool value)
    {
        // JoeWhosKevin DEPENDENT . SCUFFED
        if (value)
        {
            // holy so scuffed (WHAT IF SOMEONE GOES RLLY FAST? UHHHH RACE CONDITION!!)
            Invoke(nameof(BeginSpawning), 0.05f);
        }
        else if(inKevinDialogue)
        {
            inKevinDialogue = false;
            DisableKevin();
        }
    }

    private void BeginSpawning()
    {
        if (DialogueUI.Instance.bodyText.text == "I ain't feeling too good.." || DialogueUI.Instance.bodyText.text == "AAAAAAAAA")
        {
            DebugScript.BetterDebug("SPAWN KEVIN NOWW or uh in 1 second");
            EnableKevin();
        }
    }

    private void EnableKevin()
    {
        WiseKevin.SetActive(true);
    }
    
    private void DisableKevin()
    {
        WiseKevin.SetActive(false);
    }
}
