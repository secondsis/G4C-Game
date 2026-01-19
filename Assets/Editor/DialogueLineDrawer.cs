using System.Collections.Generic;
using Main.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CustomPropertyDrawer(typeof(DialogueLine))]
public class DialogueLineDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(pos, label, prop);

        var type = prop.FindPropertyRelative("type");
        var speaker = prop.FindPropertyRelative("speaker");
        var text = prop.FindPropertyRelative("text");
        var choices = prop.FindPropertyRelative("choices");
        var choiceActionIds = prop.FindPropertyRelative("choiceActionIds");
        var rewards = prop.FindPropertyRelative("rewardIds");

        Rect typePosition =  new Rect(pos.x, pos.y, pos.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(typePosition, type);

        Rect speakerPosition = new Rect(pos.x, pos.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, pos.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(speakerPosition, speaker);
        
        Rect textPosition = new Rect(pos.x, pos.y + EditorGUIUtility.singleLineHeight*2+ EditorGUIUtility.standardVerticalSpacing*2, pos.width, EditorGUIUtility.singleLineHeight*3);
        EditorGUI.PropertyField(textPosition, text);

        if ((DialogueLineType)type.enumValueIndex == DialogueLineType.PLAYER_CHOICE)
        {
            Rect choicesPosition = new Rect(pos.x, pos.y + EditorGUIUtility.singleLineHeight*5 + EditorGUIUtility.standardVerticalSpacing*3, pos.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(choicesPosition, choices, true);
            
            Rect choiceActionIdsPosition = new Rect(pos.x, pos.y + EditorGUIUtility.singleLineHeight*5 + EditorGUIUtility.standardVerticalSpacing*4 + EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("choices")), pos.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(choiceActionIdsPosition, choiceActionIds, true);
        }


        if ((DialogueLineType)type.enumValueIndex == DialogueLineType.REWARD)
        {
            Rect rewardsPosition = new Rect(pos.x, pos.y + EditorGUIUtility.singleLineHeight*5 + EditorGUIUtility.standardVerticalSpacing*3, pos.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(rewardsPosition, rewards, true);
        }
        
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        float h = EditorGUIUtility.singleLineHeight * 6 + 6;
        var type = prop.FindPropertyRelative("type");

        if ((DialogueLineType)type.enumValueIndex == DialogueLineType.PLAYER_CHOICE)
        {
            h += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("choices"));
            h += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("choiceActionIds"));
        }

        if ((DialogueLineType)type.enumValueIndex == DialogueLineType.REWARD)
            h += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("rewardIds"));

        return h;
    }
}