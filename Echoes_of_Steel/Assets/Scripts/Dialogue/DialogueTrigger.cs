using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueAsset[] dialogues;

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogues);
    }
}
