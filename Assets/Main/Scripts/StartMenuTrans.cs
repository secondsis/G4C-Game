using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuTrans : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
