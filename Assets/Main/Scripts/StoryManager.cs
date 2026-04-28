using System;
using System.Collections.Generic;
using Main.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    
    [SerializeField] private SpriteRenderer manipulate;
    [SerializeField] private Animator fadeOutAnim;
    [SerializeField] private List<Sprite> sprites;
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
        lastNext = DateTime.Now.Second;
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
        if (DateTime.Now.Second - lastNext < 6) return;
        lastNext = DateTime.Now.Second;
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

            return;
        }
        currIndex++;

        void Temp()
        {
            manipulate.sprite = sprites[currIndex];
        }
        
        // Animation
        fadeOutAnim.Play("FadeInOut");

        Events.OnFadeNextSprite += Temp;
    }

    public void EnterNextScene()
    {
        ao.allowSceneActivation = true;
    }
}
