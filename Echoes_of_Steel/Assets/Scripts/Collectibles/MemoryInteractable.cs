using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        GameManager.Instance.consciousMeter += 10;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(WaitForAnimation());
        
    }

    IEnumerator WaitForAnimation()
    {
        
        yield return new WaitForSeconds(3);
        
        dialogueTrigger.TriggerDialogue();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        interactMessage.SetActive(true);
    }

    private void OnTriggerExit(Collider other) { interactMessage.SetActive(false); }
}
