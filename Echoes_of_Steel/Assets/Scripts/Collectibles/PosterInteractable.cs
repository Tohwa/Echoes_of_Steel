using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private string oneLiner;
    public void Interact()
    {
        interactionStatus.AddInteractedObject(gameObject.name);
        DialogueManager.instance.PlayOneLiner(oneLiner);
        gameObject.SetActive(false);
    }
}
