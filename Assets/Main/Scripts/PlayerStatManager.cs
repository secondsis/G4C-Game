using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance;
    public int Money { get; private set; }
    public static float ReachDistance { get; private set; }
    public int playTime;
    
    // public int Level { get; private set; }

    private void Awake()
    {
        Money = PlayerPrefs.GetInt("Money");
        ReachDistance = 100f;
        
        // THIS IS A SINGLETON
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DebugMoney();
    }

    // DEBUG
    public void DebugMoney()
    {
        Money = 100;
    }

    private void SaveStats()
    {
        PlayerPrefs.SetInt("Money", Money);
        PlayerPrefs.SetInt("playTime", playTime);
        PlayerPrefs.Save();
    }

    // Only spends money if player has enough
    public bool SpendMoney(int amt)
    {
        if (Money - amt >= 0)
        {
            Money -= amt;
            return true;
        }

        return false;
    }

    public void AddMoney(int amt)
    {
        Money += amt;
    }

    private float _timer = 30f;
    private void Update()
    {
        _timer  -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = 30f;
            // NOT REaLLY ACCURATE
            playTime += 30;
        }
    }
}
