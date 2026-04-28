using Main.Scripts;
using UnityEngine;

public class StoryFadeHelper : MonoBehaviour
{
    public void OnFade()
    {
        Events.InvokeOnFadeNextSprite();
    }

    public void OnFadeOut()
    {
        StoryManager.Instance.EnterNextScene();
    }
}
