using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        public DialogueLineType type;
        public string speaker;
        public string text;
        
        [Tooltip("For PLAYER_CHOICE")]
        public List<string> choices;
        public List<string> choiceActionIds;
        
        [Tooltip("For REWARD")]
        public List<string> rewardIds;
    }
}