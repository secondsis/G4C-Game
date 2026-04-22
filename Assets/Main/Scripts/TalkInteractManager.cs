using System;
using Main.Scripts;
using UnityEngine;

public class TalkInteractManager : MonoBehaviour
{
    [SerializeField] private String GameEvent;
    public DialogueComponent dialogueComponent;
    private Interaction interactionScript;

    private void Awake()
    {
        dialogueComponent = GetComponent<DialogueComponent>();
        interactionScript = GetComponent<Interaction>();
        
        // Subscribe to dialogue event
        dialogueComponent.OnDialogueStart += SendGameEvent;
    }

    public void SetDialogue(Dialogue dialogue)
    {
        dialogueComponent.dialogue = dialogue;
    }

    private void SendGameEvent()
    {
        if (String.IsNullOrEmpty(GameEvent)) return;
        string info = Events.GameEvents[GameEvent];
        string option = info.Split(',')[0];
        string dialogueName = info.Split(',')[1];
        string joeTalkName = info.Split(',')[2];
        // Check if player has already listened to event too
        if (Dictionaries.WiseJoeTalks[joeTalkName]) return;
        JoeInstance.Instance.AddGeneralChoice(option, dialogueName);
    }
}
