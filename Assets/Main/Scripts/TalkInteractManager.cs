using System;
using Main.Scripts;
using UnityEngine;

public class TalkInteractManager : MonoBehaviour
{
    [SerializeField] private String GameEvent;
    private DialogueComponent dialogueComponent;
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
        Events.GameEvents[GameEvent] = true;
    }
}
