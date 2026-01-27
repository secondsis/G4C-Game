using System.Collections.Generic;
using Main.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }
    
    private Camera cam;
    [SerializeField] private RectTransform interactionTransform1;
    [SerializeField] private RectTransform interactionTransform2;
    
    [SerializeField] private TextMeshProUGUI interactionTitle1;
    private Interaction targetInteraction;
    private GameObject _player;
    
    private PlayerInputActions _input;

    // Whichever one is closest to the player will be the targetTransform.
    [HideInInspector] public List<Interaction> Interactions;

    private bool hasTarget = false;
    private bool blocking = false;
    
    private void Awake()
    {
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        _player = GameObject.FindGameObjectWithTag("Player");
        
        _input = G4CInputManager.G4CPlayerInput;
        _input.Enable();
        _input.Player.Enable();
        _input.Player.Interact.Enable();

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
            blocking = G4CInputManager.IsBlocked(InteractionType.INTERACTION1, OnInteract);
            Debug.Log("Updated blocking: " + blocking);
        };
    }
    
    private void OnInteract()
    {
        // I AM NOT SURE IF WHEN I PASS THIS, ALL VARIABLES ARE STATIC/CONSTANT/FROZEN
        if (targetInteraction == null) return;
        Debug.Log("Interacted");
        targetInteraction.onInteraction.Invoke();
    }

    private void UpdateTargetTransform()
    {
        if (Interactions.Count > 0 && !blocking)
        {
            if (Interactions.Count == 1)
            {
                targetInteraction = Interactions[0];
                interactionTitle1.text = targetInteraction.title;
                hasTarget = true;
                return;
            }
            
            float minDist = float.MaxValue;
            Interaction minInteraction = null;
            bool hasMinDist = false; // cheaper than checking if minInteraction is null
            foreach (Interaction interaction in Interactions)
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
                targetInteraction = minInteraction;
                hasTarget = true;
            }
            else
            {
                // This should never run
                targetInteraction = null;
                hasTarget = false;
            }
        }
        else
        {
            targetInteraction = null;
            hasTarget = false;
            interactionTransform1.gameObject.SetActive(false);
        }
    }
    
    private void Update()
    {
        UpdateTargetTransform();
        if (!hasTarget) return;
        Vector3 screenPos = cam.WorldToScreenPoint(targetInteraction.transform.position);

        // Hide if behind camera
        if (screenPos.z < 0)
        {
            interactionTransform1.gameObject.SetActive(false);
            return;
        }

        interactionTransform1.gameObject.SetActive(true);
        interactionTransform1.position = screenPos;
    }

}
