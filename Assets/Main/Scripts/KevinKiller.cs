using System;
using Main.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class KevinKiller : MonoBehaviour
{
    [SerializeField] private Interaction interaction;
    // private void Start()
    // {
    //     DialogueUI.Instance.OnDialogueUIActiveChanged += CheckToKillKevin;
    // }

    // private void CheckToKillKevin(bool value)
    // {
    //     // DialogueUI just changed 
    //     // KevinsComeback dependent!!!!! SCUFFED AS HECK
    //     if (!value && DialogueUI.Instance.bodyText.text == "And just like that, oooo he disappears oooooo")
    //     {
    //         // exit dialogue
    //         DebugScript.BetterDebug("KILL KEVIN NOWW");
    //         gameObject.SetActive(false);
    //         interaction.PromptDisable();
    //     }
    // }
}
