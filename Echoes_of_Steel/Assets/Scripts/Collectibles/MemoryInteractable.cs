using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private DialogueTrigger dialogueTrigger;
    public void Interact()
    {
        interactionStatus.AddInteractedObject(gameObject.name);
        Cursor.lockState = CursorLockMode.None;
        dialogueTrigger.TriggerDialogue();
        gameObject.SetActive(false);
    }
}
