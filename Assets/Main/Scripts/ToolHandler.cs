using System;
using Main.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class ToolHandler : MonoBehaviour
{
    // All tools must have the root be the grip.
    // The grip will allow the gameobject to be offset into the correct position.
    // The grip will always be positioned at 0,0,0 in respect to the character's right hand
    public static ToolHandler Instance;
    [SerializeField] private Transform rightHandParent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        ToolEvents.OnToolEquip += EquipInHand;
        ToolEvents.OnToolUnequip += UnequipInHand;
    }

    private void OnDisable()
    {
        ToolEvents.OnToolEquip -= EquipInHand;
        ToolEvents.OnToolUnequip -= UnequipInHand;
    }

    public void EquipInHand(GameObject tool)
    {
        // Unequip any old tools
        UnequipInHand();
        GameObject newObj = Instantiate(tool, rightHandParent);
        newObj.transform.position = rightHandParent.position;
    }

    public void UnequipInHand()
    {
        // TODO: Make this into an EVENT (OnUnequip)
        foreach (Transform child in rightHandParent)
        {
            Destroy(child.gameObject);
        }
    }

    public static bool isToolEquipped(string toolName)
    {
        // Check if the GameObject (if there is any) under rightHand is the same name
        foreach (Transform child in Instance.rightHandParent)
        {
            Debug.Log("Child: " + child.name + "\tTool: " + toolName);
            if (child.name.Substring(0, toolName.Length).Equals(toolName))
            {
                return true;
            }
        }

        return false;
    }
}
