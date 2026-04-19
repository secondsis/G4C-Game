using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class omgbigbad : MonoBehaviour
{
    [SerializeField] private AudioSource bgm;
    [SerializeField] private Animator forestAnimator;
    [SerializeField] private GameObject cooldude;
    [SerializeField] private AudioSource cooldudeSound;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Player2DMovement playerScript;
    [SerializeField] private GameObject world;
    [SerializeField] private List<GameObject> troll;
    
    private bool triggered = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Make music stop
        // Eyes disappear
        // COOLDUDE appears
        if(!triggered) PlayEvent();
        triggered = true;
    }

    private void PlayEvent()
    {
        bgm.Stop();
        forestAnimator.Play("fadeAwayForest");
        
        Invoke(nameof(COOLDUDEAppear), 5f);
    }

    private void COOLDUDEAppear()
    {
        // rahhhhhhhhhhhhhhhh randomly organized code go!
        
        cooldude.SetActive(true);
        cooldudeSound.Play();
        // lerp toward characteror smth
        var twn = Tween.Position(cooldude.transform, playerTransform.position, 10f);
        playerScript.canMove = false;
        twn.OnComplete(() =>
        {
            foreach (Transform obj in world.transform)
            {
                obj.gameObject.SetActive(false);
            }
            
            Invoke(nameof(Troll), 5f);
        });
    }

    private void Troll()
    {
        foreach (GameObject obj in troll)
        {
            obj.SetActive(true);
        }
        
        Invoke(nameof(QuitApp), 10f);
    }

    private void QuitApp()
    {
        Application.Quit();
    }
}
