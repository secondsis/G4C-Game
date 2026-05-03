using System;
using System.Collections.Generic;
using Main.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EpilogueHandler : MonoBehaviour
{
    [SerializeField] private Animator fadeOutAnim;
    [SerializeField] private List<GameObject> ThingsWithSprites;
    private int currIndex = 0;

    private long lastNext;
    
    private void Start()
    {
        lastNext = DateTime.Now.Ticks;
    }

    private void Update()
    {
        // Listeners for any, go to next scene (first frame of click)
        if (Input.anyKeyDown)
        {
            NextSprite();
        }
    }

    private void NextSprite()
    {
        // no spammys pls!
        if (Math.Abs(DateTime.Now.Ticks - lastNext) < 4 * TimeSpan.TicksPerSecond) return;
        lastNext = DateTime.Now.Ticks;
        
        Events.ClearAllOnFadeNextSprite();
        
        if (currIndex + 1 >= ThingsWithSprites.Count)
        {
            // End the scene
            // Exit
            Invoke(nameof(ExitApplication), 7f);
            return;
        }
        currIndex++;

        void Temp()
        {
            if(currIndex - 1 >= 0)
                ThingsWithSprites[currIndex-1].SetActive(false);
            ThingsWithSprites[currIndex].SetActive(true);
        }
        
        // Animation
        fadeOutAnim.Play("FadeInOut", 0, 0);

        Events.OnFadeNextSprite += Temp;
    }

    private void ExitApplication()
    {
        Application.Quit();
    }
}
