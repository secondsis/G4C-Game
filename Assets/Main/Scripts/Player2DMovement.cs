using System;
using UnityEngine;

public class Player2DMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private AudioSource footsteps;
    
    private Animator _animator;
    
    public bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        footsteps = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        float curSpeedX = canMove ? (speed) * Input.GetAxis("Horizontal") : 0;
        float curSpeedY = canMove ? (speed) * Input.GetAxis("Vertical") : 0;

        if (curSpeedX > 0.1f)
        {
            SetAnim("GreenGuyWalkRight");
        } else if (curSpeedX < -0.1f)
        {
            SetAnim("GreenGuyWalkLeft");
        } else if (curSpeedY > 0.1f)
        {
            SetAnim("GreenGuyWalkUp");
        } else if (curSpeedY < -0.1f)
        {
            SetAnim("GreenGuyWalkDown");
        }
        else
        {
            SetAnim("Idle");
        }
        
        rb.linearVelocity = new Vector2(curSpeedX, curSpeedY);
    }
    
    private void SetAnim(string state, bool forceOverride = false)
    {
        var current = _animator.GetCurrentAnimatorStateInfo(0);
        
        if (!current.IsName(state) || forceOverride)
        {
            _animator.Play(state, 0, 0.0f);
        }
    }

    public void PlayFootstep()
    {
        footsteps.Play();
    }
}
