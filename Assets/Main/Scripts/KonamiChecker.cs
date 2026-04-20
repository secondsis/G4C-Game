using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KonamiChecker : MonoBehaviour
{
    public List<KeyCode> konamiSequence = new List<KeyCode>{KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, 
        KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.A, 
        KeyCode.B, KeyCode.Return
    };

    public float delayBetween = 1f;
    private List<KeyCode> currentInputs = new List<KeyCode>();
    private float lastInput;
    
    private void Update()
    {
        int nextIndex = currentInputs.Count;
        
        if (Time.time - lastInput > delayBetween)
        {
            currentInputs.Clear();
            DebugScript.BetterDebug("cleared inputs");
        }
        
        if (Input.GetKeyDown(konamiSequence[nextIndex]))
        {
            lastInput = Time.time;
            DebugScript.BetterDebug("Next KEY BAZOOGA");
            currentInputs.Add(konamiSequence[nextIndex]);

            if (currentInputs.Count == konamiSequence.Count)
            {
                DebugScript.BetterDebug("Checking inputs");
                // Check
                foreach (KeyCode key in konamiSequence)
                {
                    if (!currentInputs.Contains(key))
                    {
                        DebugScript.BetterDebug("Failed sequence check");
                        currentInputs.Clear();
                        return;
                    }
                }
                // Yes
                DebugScript.BetterDebug("BAZINGA");
                SceneManager.LoadScene("Konami");
                currentInputs.Clear();
            }
        }
    }
}
