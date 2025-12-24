using UnityEngine;

namespace Main.Scripts
{
    public class DialogueUI : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI speakerText;
        public TMPro.TextMeshProUGUI bodyText;

        DialogueExecuter runner;

        public void StartDialogue(Dialogue dialogue)
        {
            runner = new DialogueExecuter(dialogue);
            ShowNext();
        }

        public void ShowNext()
        {
            if (!runner.HasNext)
            {
                gameObject.SetActive(false);
                return;
            }

            var line = runner.Next();
            speakerText.text = line.speaker;
            bodyText.text = line.text;
        }
    }

}