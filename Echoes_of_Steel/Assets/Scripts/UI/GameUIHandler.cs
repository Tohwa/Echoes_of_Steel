using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIHandler : MonoBehaviour
{
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private GameObject[] memories;

    private void Update()
    {
        if (interactionStatus.HasInteracted(memories[0].name))
        {
            memories[0].SetActive(true);
        }
        if (interactionStatus.HasInteracted(memories[1].name))
        {
            memories[1].SetActive(true);
        }
        if (interactionStatus.HasInteracted(memories[2].name))
        {
            memories[2].SetActive(true);
        }
        if (interactionStatus.HasInteracted(memories[3].name))
        {
            memories[3].SetActive(true);
        }
        if (interactionStatus.HasInteracted(memories[4].name))
        {
            memories[4].SetActive(true);
        }
        if (interactionStatus.HasInteracted(memories[5].name))
        {
            memories[5].SetActive(true);
        }
    }
}
