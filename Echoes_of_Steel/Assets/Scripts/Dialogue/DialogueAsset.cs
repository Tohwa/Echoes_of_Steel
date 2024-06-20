using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    public Dialogue[] sentences;
    public Character[] characters;
    public Sprite memoryImage;
    public bool isChoiceDialogue;
    public bool isEndDialogue;

}

[System.Serializable]
public class Dialogue
{
    public int characterId;
    [TextArea]
    public string dialogue;
}

[System.Serializable]
public class Character
{
    public string name;
    public Sprite sprite;
}
