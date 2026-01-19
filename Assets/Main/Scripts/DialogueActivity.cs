using System;
using UnityEngine;

public class DialogueActivity : MonoBehaviour
{
    public event Action<bool> OnActiveChanged;
    void OnEnable()  => OnActiveChanged?.Invoke(true);
    void OnDisable() => OnActiveChanged?.Invoke(false);
}
