using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIHandler : MonoBehaviour
{
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private GameObject[] memories;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Image memoryImage;
    [SerializeField] private GameObject panel;
    [SerializeField] private Animator journalAnim;

    private void Start()
    {
        //for (int i = 0; i < buttons.Length - 1; i++)
        //{
        //    buttons[i].onClick.AddListener(delegate { OpenMemory(i); });
        //}

        buttons[0].onClick.AddListener(delegate { OpenMemory(0); });
        buttons[1].onClick.AddListener(delegate { OpenMemory(1); });
        buttons[2].onClick.AddListener(delegate { OpenMemory(2); });
        buttons[3].onClick.AddListener(delegate { OpenMemory(3); });
        buttons[4].onClick.AddListener(delegate { OpenMemory(4); });
        buttons[5].onClick.AddListener(delegate { OpenMemory(5); });
    }


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

    public void OpenMemory(int _value)
    {
        memoryImage.sprite = memories[_value].GetComponentInChildren<Image>().sprite;
        journalAnim.SetBool("IsMemoryOpen", true);
        panel.SetActive(true);
    }

    public void CloseMemory()
    {
        journalAnim.SetBool("IsMemoryOpen", false);
        panel.SetActive(false);
    }
}
