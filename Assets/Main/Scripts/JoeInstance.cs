using System;
using System.Collections.Generic;
using Main.Scripts;
using UnityEngine;

public class JoeInstance : MonoBehaviour
{
    public static JoeInstance Instance;
    
    [SerializeField] private NPCManager.NPC npc;
    [SerializeField] private float offset;
    private List<Dialogue> dialogues;
    
    private GameObject TalkInteractPrefab;
    private GameObject TalkInteractObject;
    private TalkInteractManager TalkInteractManager;
    private Dialogue GeneralTalk;

    private void Awake()
    {
        TalkInteractPrefab = Resources.Load<GameObject>("Prefabs/TalkInteract");
        // NPC might have multiple dialogue
        dialogues = npc.dialogue;
        
        // Create an interact prompt (if dialogue)
        if (dialogues != null)
        {
            TalkInteractObject = Instantiate(TalkInteractPrefab, gameObject.transform);
            TalkInteractObject.transform.localPosition = Vector3.zero;
            TalkInteractObject.transform.localPosition += new Vector3(0, offset, 0);
            TalkInteractManager = TalkInteractObject.GetComponent<TalkInteractManager>();
            TalkInteractManager.SetDialogue(dialogues[0]);
            // After his welcome, all dialogue goes to the "General" talk, which can be modified after game events.
            GeneralTalk = dialogues[1];
            TalkInteractManager.dialogueComponent.OnDialogueStart += FinishWelcome;
        }
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void FinishWelcome()
    {
        if (TalkInteractManager.dialogueComponent.dialogue == GeneralTalk) return;
        // Set to General talk
        TalkInteractManager.SetDialogue(GeneralTalk);
        // GeneralTalk.lines[1].choices = 
    }

    public void AddGeneralChoice(string choiceTitle, string choiceAction)
    {
        if (GeneralTalk.lines[1].choices.Contains(choiceTitle)) return;
        GeneralTalk.lines[1].choices.Add(choiceTitle);
        GeneralTalk.lines[1].choiceActionIds.Add(choiceAction);
    }

    public void RemoveGeneralChoice(string choiceTitle, string choiceAction)
    {
        GeneralTalk.lines[1].choices.Remove(choiceTitle);
        GeneralTalk.lines[1].choiceActionIds.Remove(choiceAction);
    }
}
