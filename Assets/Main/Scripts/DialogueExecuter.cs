namespace Main.Scripts
{
    public class DialogueExecuter
    {
        public Dialogue dialogue;
        int index;

        public DialogueExecuter(Dialogue dialogue)
        {
            this.dialogue = dialogue;
        }

        public bool HasNext => index < dialogue.lines.Count;

        public DialogueLine GetCurrent()
        {
            return dialogue.lines[index];
        }
        
        public DialogueLine Next()
        {
            return dialogue.lines[index++];
        }
    }

}