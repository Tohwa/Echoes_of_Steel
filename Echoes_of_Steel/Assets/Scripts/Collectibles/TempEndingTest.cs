using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEndingTest : MonoBehaviour
{
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private MeshRenderer MeshRender;

    private void Start()
    {
        MeshRender = GetComponent<MeshRenderer>();
    }


    private void Update()
    {
        if (interactionStatus.HasInteracted("RedKey"))
        {
            MeshRender.material.color = Color.red;
        }

        if (interactionStatus.HasInteracted("BlueKey"))
        {
            MeshRender.material.color = Color.blue;
        }

        if (interactionStatus.HasInteracted("GreenKey"))
        {
            MeshRender.material.color = Color.green;
        }

        if (interactionStatus.HasInteracted("RedKey") && interactionStatus.HasInteracted("GreenKey"))
        {
            MeshRender.material.color = Color.red + Color.green;
        }

        if (interactionStatus.HasInteracted("RedKey") && interactionStatus.HasInteracted("BlueKey"))
        {
            MeshRender.material.color = Color.red + Color.blue;
        }

        if (interactionStatus.HasInteracted("BlueKey") && interactionStatus.HasInteracted("GreenKey"))
        {
            MeshRender.material.color = Color.blue + Color.green;
        }

        if(interactionStatus.HasInteracted("BlueKey") && interactionStatus.HasInteracted("GreenKey") && interactionStatus.HasInteracted("RedKey"))
        {
            MeshRender.material.color = Color.blue + Color.green + Color.red;
        }
    }
}
