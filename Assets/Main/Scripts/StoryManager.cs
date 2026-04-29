using System;
using System.Collections.Generic;
using Main.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    
    [SerializeField] private Animator fadeOutAnim;
    [SerializeField] private List<GameObject> sprites;
    private AsyncOperation ao;
    private int currIndex = 0;

    private long lastNext;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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
         Debug.Log("NextSprite");
        
        Events.ClearAllOnFadeNextSprite();
        
        if (currIndex + 1 >= sprites.Count)
        {
            // End the scene
            // Fade out, go to next
            ao = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            ao.allowSceneActivation = false;
            // Fade
            fadeOutAnim.Play("FadeOut");
            Debug.Log("FadeOut - END");
            return;
        }
        currIndex++;

        void Temp()
        {
            if(currIndex - 1 >= 0)
                sprites[currIndex-1].SetActive(false);
            sprites[currIndex].SetActive(true);
        }
        
        // Animation
        Debug.Log("Play FadeInOut");
        fadeOutAnim.Play("FadeInOut", 0, 0);

        Events.OnFadeNextSprite += Temp;
    }

    public void EnterNextScene()
    {
        ao.allowSceneActivation = true;
    }
}
