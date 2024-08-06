using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private GameObject interactMessage;
    public void Interact()
    {
        interactionStatus.AddInteractedObject(gameObject.name);
        Cursor.lockState = CursorLockMode.None;
        dialogueTrigger.TriggerDialogue();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        interactMessage.SetActive(true);
    }

    private void OnTriggerExit(Collider other) { interactMessage.SetActive(false); }
}
