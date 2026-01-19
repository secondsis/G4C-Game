using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoiceDictionary : MonoBehaviour
{
    private static Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public static void Register(string id, Action action)
    {
        actions[id] = action;
    }

    public static void Invoke(string id)
    {
        if (actions.TryGetValue(id, out var action))
            action.Invoke();
        else
            Debug.LogWarning($"No action registered for ID {id}");
    }
}
