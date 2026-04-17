using System.Collections.Generic;
using Main.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// Sometimes this can be buggy 
// - Prompt may float upwards
// - "Ghost" prompt left behind
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }
    
    private Camera cam;
    
    [SerializeField] private RectTransform interactionTransform1;
    [SerializeField] private TextMeshProUGUI interactionTitle1;
    [SerializeField] private TextMeshProUGUI interactionKeybind1;
    
    [SerializeField] private RectTransform interactionTransform2;
    [SerializeField] private TextMeshProUGUI interactionTitle2;
    [SerializeField] private TextMeshProUGUI interactionKeybind2;

    private Interaction targetInteraction1;
    private Interaction targetInteraction2;
    private GameObject _player;
    
    private PlayerInputActions _input;

    // Whichever one is closest to the player will be the targetTransform.
    [FormerlySerializedAs("Interactions")] [HideInInspector] public List<Interaction> Interactions1;
    [FormerlySerializedAs("Interactions")] [HideInInspector] public List<Interaction> Interactions2;
    
    private bool hasTarget1 = false;
    private bool blocking1 = false;
    private bool hasTarget2 = false;
    private bool blocking2 = false;
    
    private void Awake()
    {
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        _player = GameObject.FindGameObjectWithTag("Player");
        
        _input = G4CInputManager.G4CPlayerInput;
        _input.Enable();
        _input.Player.Enable();
        _input.Player.Interact.Enable();
        _input.Player.Interact2.Enable();

        // TODO: Make this only be called when necessary
        G4CInputManager.RegisterInteract(InteractionType.INTERACTION1, OnInteract1);
        G4CInputManager.RegisterInteract(InteractionType.INTERACTION2, OnInteract2, 2);
        
        // THIS IS A SINGLETON
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void Start()
    {
        // Potential Blocker: DIALOGUE
        DialogueUI.Instance.OnDialogueUIActiveChanged += b =>
        {
            blocking1 = G4CInputManager.IsBlocked(InteractionType.INTERACTION1, OnInteract1);
            Debug.Log("Updated blocking: " + blocking1);
        };
    }
    
    private void OnInteract1()
    {
        // I AM NOT SURE IF WHEN I PASS THIS, ALL VARIABLES ARE STATIC/CONSTANT/FROZEN
        if (targetInteraction1 == null) return;
        DebugScript.BetterDebug("Interacted");
        targetInteraction1.onInteraction.Invoke();
    }
    
    private void OnInteract2()
    {
        DebugScript.BetterDebug("OnInteract2 Called/");
        // I AM NOT SURE IF WHEN I PASS THIS, ALL VARIABLES ARE STATIC/CONSTANT/FROZEN
        if (targetInteraction2 == null) return;
        DebugScript.BetterDebug("Interacted2");
        targetInteraction2.onInteraction.Invoke();
    }

    // Sets the target interact based on shortest distance from player
    private void UpdateTargetTransform1()
    {
        // This means there is another interaction blocking this, OR there are no interactions, OR manual blocking (cutscenes)
        if (G4CInputManager.GetCurrentInteractionType() > InteractionType.INTERACTION1 || Interactions1.Count == 0 || blocking1)
        {
            targetInteraction1 = null;
            hasTarget1 = false;
            interactionTransform1.gameObject.SetActive(false);
            return;
        }
        
        if (Interactions1.Count == 1)
        {
            targetInteraction1 = Interactions1[0];
            interactionTitle1.text = targetInteraction1.title;
            // Also the interaction button key
            interactionKeybind1.text = targetInteraction1.interactionKeybind;
            hasTarget1 = true;
            return;
        }
        
        // Only show the prompt closest to player
        
        float minDist = float.MaxValue;
        Interaction minInteraction = null;
        bool hasMinDist = false; // less overhead than checking if minInteraction is null
        // Find shortest distance
        foreach (Interaction interaction in Interactions1)
        {
            float currDist = Vector3.Distance(_player.transform.position, interaction.transform.position);
            if (currDist < minDist)
            {
                minDist = currDist;
                minInteraction = interaction;
                hasMinDist = true;
            }
        }

        if (hasMinDist)
        {
            // Sets the target interaction to be the one closest in distance
            if (hasTarget1)
            {
                interactionTransform1.gameObject.SetActive(false);
            }
            targetInteraction1 = minInteraction;
            hasTarget1 = true;
        }
        else
        {
            // This should never run
            DebugScript.BetterDebug("THIS SHOULD NEVER RUN.");
            targetInteraction1 = null;
            hasTarget1 = false;
        }
    }
    
    private void UpdateTargetTransform2()
    {
        if (Interactions2.Count > 0 && !blocking2)
        {
            if (Interactions2.Count == 1)
            {
                if (targetInteraction2 == Interactions2[0]) return;
                targetInteraction2 = Interactions2[0];
                interactionTitle2.text = targetInteraction2.title;
                DebugScript.BetterDebug("Setting targetInteraction2: " + targetInteraction2.title);
                // Also the interaction button
                interactionKeybind2.text = targetInteraction2.interactionKeybind;
                hasTarget2 = true;
                return;
            }
            
            float minDist = float.MaxValue;
            Interaction minInteraction = null;
            bool hasMinDist = false; // cheaper than checking if minInteraction is null
            foreach (Interaction interaction in Interactions2)
            {
                float currDist = Vector3.Distance(_player.transform.position, interaction.transform.position);
                if (currDist < minDist)
                {
                    minDist = currDist;
                    minInteraction = interaction;
                    hasMinDist = true;
                }
            }

            if (hasMinDist)
            {
                DebugScript.BetterDebug("Has minDist Setting targetInteraction2: " + minInteraction.title);
                targetInteraction2 = minInteraction;
                interactionTitle2.text = targetInteraction2.title;
                interactionKeybind2.text = targetInteraction2.interactionKeybind;
                hasTarget2 = true;
            }
            else
            {
                // This should never run
                targetInteraction2 = null;
                hasTarget2 = false;
            }
        }
        else
        {
            targetInteraction2 = null;
            hasTarget2 = false;
            interactionTransform2.gameObject.SetActive(false);
        }
    }
    
    private void Update()
    {
        UpdateTargetTransform1();
        // UpdateTargetTransform2(); // we don't currently use interact2
        
        if (hasTarget1)
        {
            // Get the screen position of the world object
            Vector3 screenPos = cam.WorldToScreenPoint(targetInteraction1.transform.position);
            // DebugScript.BetterDebug(cam.transform.position);
            // WHY do these numbers go crazy even though position is the same
            // DebugScript.BetterDebug(screenPos);
            //
            // Hide if behind camera
            if (screenPos.z < 0)
            {
                interactionTransform1.gameObject.SetActive(false);
                return;
            }
            //
            // // Enable the interact prompt and place it in its corresponding screen position
            interactionTransform1.gameObject.SetActive(true);
            // Vector2 localPos;
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(interactionTransform1, screenPos, cam,
            //     out localPos);
            interactionTransform1.transform.position = screenPos;
        } //else if (hasTarget2)
        // {
        //     Vector3 screenPos = cam.WorldToScreenPoint(targetInteraction2.transform.position);
        //
        //     // Hide if behind camera
        //     if (screenPos.z < 0)
        //     {
        //         interactionTransform2.gameObject.SetActive(false);
        //         return;
        //     }
        //
        //     interactionTransform2.gameObject.SetActive(true);
        //     interactionTransform2.position = screenPos;
        // }

    }

}
