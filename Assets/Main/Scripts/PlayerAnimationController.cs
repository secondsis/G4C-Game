using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void SetAnim(string state)
    {
        anim.CrossFade(state, 0.1f); // 0.1 sec blend
    }

    public void PlayJumpAnim()
    {
        SetAnim("Jump");
        Debug.Log("Jump");
    }

    public void PlayWalkAnim()
    {
        SetAnim("Walk");
        Debug.Log("Walk");
    }

    public void PlayIdleAnim()
    {
        SetAnim("Idle");
        Debug.Log("Idle");
    }

    public void PlayRunAnim()
    {
        SetAnim("Run");
        Debug.Log("Run");
    }
}
