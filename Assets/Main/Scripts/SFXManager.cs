using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    [SerializeField] private AudioSource buySfx;
    [SerializeField] private AudioSource harvestSfx;
    [SerializeField] private AudioSource sellSfx;
    [SerializeField] private AudioSource shopEnterSfx;

    private void Awake()
    {
        
        // THIS IS A SINGLETON
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void BuySfx()
    {
        buySfx.Play();
        DebugScript.BetterDebug("what");
    }

    public void SellSfx()
    {
        sellSfx.Play();
    }

    public void HarvestSfx()
    {
        harvestSfx.Play();
    }

    public void ShopEnterSfx()
    {
        shopEnterSfx.time = 2f;
        shopEnterSfx.Play();
        
    }
}
