using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts
{
    [CreateAssetMenu(menuName = "Dialogue/Conversation")]
    public class Dialogue : ScriptableObject
    {
        public List<DialogueLine> lines;
    }

    [System.Serializable]
    public class DialogueLine
    {
        public string speaker;
        [TextArea(2, 5)]
        public string text;
    }

}