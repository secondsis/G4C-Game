using System;
using UnityEngine;

public class WorldCanvasRotator : MonoBehaviour
{
    private GameObject playerObject;

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        gameObject.transform.LookAt(playerObject.transform);
    }
}
