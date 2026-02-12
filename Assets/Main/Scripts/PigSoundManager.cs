using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class PigSoundManager : MonoBehaviour
{
    // Jolly pigs :DD

    [SerializeField] private AudioResource[] normalPigSounds;
    [SerializeField] private AudioResource[] evilPigSounds;

    private AudioSource pigSource;
    
    private int evilCounter = 0;
    private float pigInterval = 3f;

    private void Start()
    {
        pigSource = GetComponent<AudioSource>();
        // needs nameof for getting the string version of the method withtout erroring
        InvokeRepeating(nameof(PlayPigNoise), 0f, pigInterval);
    }

    private void PlayPigNoise()
    {
        evilCounter += Random.Range(0, 2);
        if (evilCounter >= 100)
        {
            pigSource.resource = evilPigSounds[Random.Range(0, evilPigSounds.Length)];
            evilCounter = 0;
        }
        else
        {
            pigSource.resource = normalPigSounds[Random.Range(0, normalPigSounds.Length)];
        }
        
        pigSource.Play();
    }
}
