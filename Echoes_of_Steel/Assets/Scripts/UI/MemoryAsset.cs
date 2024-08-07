using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Memory Asset", menuName = "UI/Memory Asset", order = 1)]

public class MemoryAsset : ScriptableObject
{
    [TextArea]
    public string[] infoTexts;
    public Sprite[] memorySprites;
}
