using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Settings", menuName = "Dialogue/Dialogue Settings", order = 1)]
public class DialogSettings : ScriptableObject
{
    public Character[] characters;
}

[System.Serializable]
public class Character
{
    public string name;
    public Sprite sprite;
}
