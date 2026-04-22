using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Main.Scripts
{
    public class DialogueUI : MonoBehaviour
    {
        public static DialogueUI Instance { get; private set; }
        public event Action<bool> OnDialogueUIActiveChanged;
        
        public TMPro.TextMeshProUGUI speakerText;
        public TMPro.TextMeshProUGUI bodyText;
        public Transform optionPanel;
        [SerializeField] private GameObject optionPrefab;

        private DialogueExecuter _runner;
        
        private PlayerInputActions _input;

        [SerializeField] private DialogueActivity _dialogueActivity;
        public bool InDialogue => _dialogueActivity.gameObject.activeSelf;

        private void Awake()
        {
            _input = G4CInputManager.G4CPlayerInput;
            _input.Enable();
            _dialogueActivity.gameObject.SetActive(false);
            
            // THIS IS A SINGLETON
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void StartDialogue(Dialogue dialogue)
        {
            if (InDialogue)
            {
                Debug.LogWarning("DialogueUI is already in dialogue!");
                return;
            }
            _dialogueActivity.gameObject.SetActive(true);
            OnDialogueUIActiveChanged?.Invoke(true);
            DebugScript.BetterDebug("Starting dialogue...");
            // STOP MOVEMENT
            PlayerMovement.ToggleMovement(false);
            _runner = new DialogueExecuter(dialogue);
            G4CInputManager.RegisterInteract(InteractionType.DIALOGUE, OnInteract);
            ShowNext();
        }

        public void ShowNext()
        {
            DebugScript.BetterDebug("Show Next");
            if (!_runner.HasNext)
            {
                DebugScript.BetterDebug("No Next");
                // REENABLE MOVEMENT
                PlayerMovement.ToggleMovement(true);
                G4CInputManager.RemoveInteract(InteractionType.DIALOGUE, OnInteract);
                _dialogueActivity.gameObject.SetActive(false);
                OnDialogueUIActiveChanged?.Invoke(false);
                return;
            }

            var line = _runner.Next();
            speakerText.text = line.speaker;
            bodyText.text = line.text;
            if (line.type == DialogueLineType.PLAYER_CHOICE)
            {
                // HOTKEY SHOULD NOT PROGRESS DIALOGUE WHEN CHOICE / why does it still progress?
                // Turns off dialogue temporarily if there is a dialogue choice
                G4CInputManager.SetInteract(InteractionType.DIALOGUE, OnInteract, false);
                var choices = line.choices;
                var choiceActionIds = line.choiceActionIds;
                if (choices == null)
                {
                    Debug.LogWarning("Player choices was not initialized!");
                    return;
                }

                for (int i = 0; i < choices.Count; i++)
                {
                    var choice = choices[i];
                    string choiceActionId = choiceActionIds[i];
                    // MAKE A DICITONARY OF ACTIONIDS TO ACTIONS!!!
                    Action choiceAction = () => DialogueChoiceDictionary.Invoke(choiceActionId);
                    // Create a new Option Button
                    GameObject newOption = Instantiate(optionPrefab, optionPanel);
                    Transform button = newOption.transform.Find("Button");
                    
                    // Add the OnClick listener to the event of this Option Button
                    Button.ButtonClickedEvent newButtonEvent = new Button.ButtonClickedEvent();
                    newButtonEvent.AddListener(() =>
                    {
                        G4CInputManager.SetInteract(InteractionType.DIALOGUE, OnInteract, true);
                        ShowNext();
                        choiceAction.Invoke();
                        foreach (Transform option in optionPanel)
                        {
                            Destroy(option.gameObject);
                        }
                    });
                    button.GetComponent<Button>().onClick = newButtonEvent;
                    
                    
                    // Set the text of this Option Button
                    button.Find("Text").GetComponent<TextMeshProUGUI>().text = choice;
                }
            }
        }
        
        // void HandleDialogueUI(bool isEnabled)
        // {
        //     if (isEnabled)
        //     {
        //         G4CInputManager.RegisterInteract(InteractionType.DIALOGUE, OnInteract);
        //     }
        //     else
        //     {
        //         G4CInputManager.RemoveInteract(InteractionType.DIALOGUE, OnInteract);
        //     }
        // }
        //
        // private void OnEnable()
        // {
        //     _dialogueActivity.OnActiveChanged += HandleDialogueUI;
        // }
        //
        // private void OnDisable()
        // {
        //     _dialogueActivity.OnActiveChanged -= HandleDialogueUI;
        // }

        // This is never called
        private void OnInteract()
        {
            DebugScript.BetterDebug("OnInteract - DialogueUI");
            ShowNext();
            // Make typewriting effect .. later
        }
        
        public static void LaunchDialogue(string fileName)
        {
            var s = Resources.Load<Dialogue>("Dialogue/" + fileName);
            if (s)
            {
                Instance.StartDialogue(s);
            }
            else
            {
                Debug.LogWarning("No Dialogue Found for: " + fileName);
            }
        }
    }

}