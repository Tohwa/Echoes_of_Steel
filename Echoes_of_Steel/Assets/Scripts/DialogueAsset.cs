using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    public string mainCharName;
    public string sideCharName;
    [TextArea]
    public string[] sentences;
}
