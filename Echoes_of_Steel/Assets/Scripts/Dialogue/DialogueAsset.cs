using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Asset", menuName = "Dialogue/Dialogue Asset", order = 2)]
public class DialogueAsset : ScriptableObject
{
    public Dialogue[] sentences;
    public Sprite memoryImage;
    public bool isChoiceDialogue;
    public bool isEndDialogue;
    public bool enableShooting;
    public bool enableShielding;
}

[System.Serializable]
public class Dialogue
{
    public int characterId;
    [TextArea]
    public string dialogue;
}


