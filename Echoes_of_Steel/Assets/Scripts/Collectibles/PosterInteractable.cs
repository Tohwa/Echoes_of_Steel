using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private string oneLiner;
    [SerializeField] private GameObject interactMessage;


    public void Interact()
    {
        interactionStatus.AddInteractedObject(gameObject.name);
        DialogueManager.instance.PlayOneLiner(oneLiner);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        interactMessage.SetActive(true);
    }

    private void OnTriggerExit(Collider other) { interactMessage.SetActive(false); }

}
