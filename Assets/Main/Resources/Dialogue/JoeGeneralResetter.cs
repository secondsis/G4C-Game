using System;
using System.Collections.Generic;
using Main.Scripts;
using UnityEngine;

public class JoeGeneralResetter : MonoBehaviour
{
    private Dialogue JoeGeneral;

    private void Awake()
    {
        JoeGeneral = Resources.Load<Dialogue>("Dialogue/JoeGeneral");
        JoeGeneral.lines[0] = new DialogueLine(DialogueLineType.NORMAL, "Wise Joe", "Hey Joey!");
        JoeGeneral.lines[1] = new DialogueLine(DialogueLineType.PLAYER_CHOICE, "Wise Joe", 
            "Anything you need?", new List<String>{"Nothing", "Why empty?"}, new List<String>{"Default", "GotEmpty"});
    }
}
