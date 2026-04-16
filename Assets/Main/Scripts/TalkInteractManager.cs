using System;
using Main.Scripts;
using UnityEngine;

public class TalkInteractManager : MonoBehaviour
{
    private DialogueComponent dialogueComponent;
    private Interaction interactionScript;

    private void Awake()
    {
        dialogueComponent = GetComponent<DialogueComponent>();
        interactionScript = GetComponent<Interaction>();
    }

    public void SetDialogue(Dialogue dialogue)
    {
        dialogueComponent.dialogue = dialogue;
    }
}
