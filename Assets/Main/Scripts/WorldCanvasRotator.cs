using System;
using UnityEngine;

// (Unused)
public class Rotator : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Vector3 directionOfRotation = Vector3.up;
    [SerializeField] private float speed = 1f;
    
    private void FixedUpdate()
    {
        _gameObject.transform.Rotate(directionOfRotation, 1f * speed);
    }
}
