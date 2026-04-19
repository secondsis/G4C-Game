using System;
using Main.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

public class DialogueComponent : MonoBehaviour
{
    // Get Dialogue from the Asset Objects
    public Dialogue dialogue;
    [SerializeField] private DialogueUI _dialogueUI; // Create a Prefab for this. There's only one reference for it.
    public event Action OnDialogueStart;
    
    private void Awake()
    {
        if (_dialogueUI == null)
        {
            _dialogueUI = FindFirstObjectByType<DialogueUI>();
        }
    }
    
    public void StartDialogue()
    {
        Debug.Log("Start Dialogue from Component");
        OnDialogueStart?.Invoke();
        _dialogueUI.StartDialogue(dialogue);
    }
}
