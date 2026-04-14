namespace Main.Scripts
{
    public class NPCManager
    {
        [System.Serializable]
        public class NPC
        {
            public int id;
            public string name;
            public int walkSpeed;
            // add walkpath variable
            public Dialogue dialogue;

            // Other info to add, maybe specific characteristics
            public string config;
        }
        
        
    }
}