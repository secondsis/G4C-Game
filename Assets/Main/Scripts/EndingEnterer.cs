using System;
using Main.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingEnterer : MonoBehaviour
{
    public static EndingEnterer Instance;
    
    private Animator fadeAnimator;
    private AsyncOperation async;
    
    private float timer = 0.0f;
    private float duration = 5.0f;
    private bool endingReady = false;
    private bool ending = false;

    private void Awake()
    {
        fadeAnimator = GetComponent<Animator>();
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartEnding()
    {
        endingReady = true;
    }
    
    private void Update()
    {
        if (!endingReady) return;
        
        timer +=  Time.deltaTime;
        if (timer >= duration)
        {
            timer = 0.0f;
            InvokeEnding();
        }
    }

    private void InvokeEnding()
    {
        // attempt every 5s (cool solution called scuffed)
        // Check if in dialogue
        if (DialogueUI.Instance.InDialogue) return;

        if (ending) return; // prob still loading scene
        ending = true;
        
        // Fadeout
        fadeAnimator.Play("FadeOut");
        
        // Load the scene
        AsyncOperation ao = SceneManager.LoadSceneAsync("Ending");
        ao.allowSceneActivation = false;
    }

    // call from fadeAnimator event. 
    public void LoadEndingScene()
    {
        async.allowSceneActivation = true;
    }
}
