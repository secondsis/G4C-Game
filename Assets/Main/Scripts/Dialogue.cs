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
        
        // Normal dialogue line
        public DialogueLine(DialogueLineType type, string speaker, string text)
        {
            this.type = type;
            this.speaker = speaker;
            this.text = text;
        }
        
        // Choice dialogue line
        public DialogueLine(DialogueLineType type, string speaker, string text, List<string> choices, List<string> choiceActionIds)
        {
            this.type = type;
            this.speaker = speaker;
            this.text = text;
            this.choices = choices;
            this.choiceActionIds = choiceActionIds;
        }
    }
}