namespace Main.Scripts
{
    public class DialogueExecuter
    {
        Dialogue dialogue;
        int index;

        public DialogueExecuter(Dialogue dialogue)
        {
            this.dialogue = dialogue;
        }

        public bool HasNext => index < dialogue.lines.Count;

        public DialogueLine Next()
        {
            return dialogue.lines[index++];
        }
    }

}