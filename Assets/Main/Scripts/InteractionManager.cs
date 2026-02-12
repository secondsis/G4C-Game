using System.Collections.Generic;
using Main.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
        G4CInputManager.RegisterInteract(InteractionType.INTERACTION1, OnInteract);
        
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
            blocking1 = G4CInputManager.IsBlocked(InteractionType.INTERACTION1, OnInteract);
            Debug.Log("Updated blocking: " + blocking1);
        };
    }
    
    private void OnInteract()
    {
        // I AM NOT SURE IF WHEN I PASS THIS, ALL VARIABLES ARE STATIC/CONSTANT/FROZEN
        if (targetInteraction1 == null) return;
        DebugScript.BetterDebug("Interacted");
        targetInteraction1.onInteraction.Invoke();
    }

    private void UpdateTargetTransform1()
    {
        if (Interactions1.Count > 0 && !blocking1)
        {
            if (Interactions1.Count == 1)
            {
                targetInteraction1 = Interactions1[0];
                interactionTitle1.text = targetInteraction1.title;
                // Also the interaction button
                interactionKeybind1.text = targetInteraction1.interactionKeybind;
                hasTarget1 = true;
                return;
            }
            
            float minDist = float.MaxValue;
            Interaction minInteraction = null;
            bool hasMinDist = false; // cheaper than checking if minInteraction is null
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
                targetInteraction1 = minInteraction;
                hasTarget1 = true;
            }
            else
            {
                // This should never run
                targetInteraction1 = null;
                hasTarget1 = false;
            }
        }
        else
        {
            targetInteraction1 = null;
            hasTarget1 = false;
            interactionTransform1.gameObject.SetActive(false);
        }
    }
    
    private void UpdateTargetTransform2()
    {
        if (Interactions2.Count > 0 && !blocking2)
        {
            if (Interactions2.Count == 1)
            {
                targetInteraction2 = Interactions2[0];
                interactionTitle2.text = targetInteraction2.title;
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
                targetInteraction2 = minInteraction;
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
        UpdateTargetTransform2();
        
        if (hasTarget1)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(targetInteraction1.transform.position);

            // Hide if behind camera
            if (screenPos.z < 0)
            {
                interactionTransform1.gameObject.SetActive(false);
                return;
            }

            interactionTransform1.gameObject.SetActive(true);
            interactionTransform1.position = screenPos;
        } else if (hasTarget2)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(targetInteraction2.transform.position);

            // Hide if behind camera
            if (screenPos.z < 0)
            {
                interactionTransform2.gameObject.SetActive(false);
                return;
            }

            interactionTransform2.gameObject.SetActive(true);
            interactionTransform2.position = screenPos;
        }

    }

}
