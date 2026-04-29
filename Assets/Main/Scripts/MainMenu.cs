using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator fadePanelAnimator;
    
    [SerializeField] private GameObject SettingsObject;
    [SerializeField] private AudioSource stinkySFx;
    
    public void OnSettingsClick()
    {
        SettingsObject.SetActive(true);
        stinkySFx.Play();
        Invoke(nameof(DisableSettingsObject), 3f);
    }

    private void DisableSettingsObject()
    {
        SettingsObject.SetActive(false);
    }

    public void OnStartClick()
    {
        fadePanelAnimator.Play("FadeOut", 0,0);
        fadePanelAnimator.gameObject.GetComponent<Image>().raycastTarget = true;
    }
}
