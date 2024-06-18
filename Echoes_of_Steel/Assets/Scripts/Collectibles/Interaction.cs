using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private DialogueTrigger DiaTrigger; 
    private bool isPlayerInRange;

    void Start()
    {
        isPlayerInRange = false;
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!interactionStatus.HasInteracted(gameObject.name))
            {
                Interact();
            }
        }
    }

    private void Interact()
    {
        Debug.Log("Interagiert mit: " + gameObject.name);
        interactionStatus.AddInteractedObject(gameObject.name);
        Cursor.lockState = CursorLockMode.None;
        DiaTrigger.TriggerDialogue();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Spieler ist im Bereich von: " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Spieler hat den Bereich von: " + gameObject.name + " verlassen");
        }
    }
}
