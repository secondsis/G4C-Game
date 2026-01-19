using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts
{
    [CreateAssetMenu(menuName = "Quests/Quest")]
    public class Quests : ScriptableObject
    {
        public List<Quest> questList;
    }

    [System.Serializable]
    public class Quest
    {
        public int id;
        public string title;
        [TextArea(2, 5)]
        public string description;
        
        public List<Objective> objectives;
        public List<Reward> rewards;
    }

    [System.Serializable]
    public class Objective
    {
        public ObjectiveState objectiveState;
        public string objectiveDescription;
        // EVENT(S) TO LISTEN TO THAT WOULD COMPLETE/PROGRESS THIS OBJECTIVE
        // idk
    }
    
    public enum ObjectiveState { NOT_STARTED, IN_PROGRESS, COMPLETED }

    [System.Serializable]
    public class Reward
    {
        public string itemId;
        public int quantity;
    }
}