using System;
using System.Collections.Generic;
using Main.Scripts;
using UnityEngine;

public class NPCInstance : MonoBehaviour
{
    [SerializeField] private NPCManager.NPC npc;
    [SerializeField] private float offset;
    private List<Dialogue> dialogues;
    

    private GameObject TalkInteractPrefab;
    private GameObject TalkInteractObject;
    private TalkInteractManager TalkInteractManager;
    // public int id;
    // public string name;
    // public int walkSpeed;
    // // add walkpath variable
    // public Dialogue dialogue;
    //
    // // Other info to add, maybe specific characteristics
    // public string config;
    
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
        }
    }
}
