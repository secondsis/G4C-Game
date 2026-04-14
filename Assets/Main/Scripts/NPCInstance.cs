using System;
using Main.Scripts;
using UnityEngine;

public class NPCInstance : MonoBehaviour
{
    [SerializeField] private NPCManager.NPC npc;

    private GameObject TalkInteractPrefab;
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
        TalkInteractPrefab = Resources.Load<GameObject>("Prefabs/NPC/TalkInteract");
        // Create an interact prompt (if dialogue)
        if (npc.dialogue)
        {
            Instantiate(TalkInteractPrefab, gameObject.transform.parent.transform);
        }
    }
}
