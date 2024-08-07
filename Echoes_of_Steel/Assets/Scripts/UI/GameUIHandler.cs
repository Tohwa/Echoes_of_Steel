using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIHandler : MonoBehaviour
{
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private MemoryAsset memoryAsset;
    [SerializeField] private GameObject[] memories;
    [SerializeField] private TextMeshProUGUI memoryText;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Image memoryImage;
    [SerializeField] private Image popUpImage;
    [SerializeField] private TextMeshProUGUI popUpText;
    [SerializeField] private Sprite corruptedImage;
    [SerializeField] private GameObject panel;
    [SerializeField] private Animator journalAnim;
    [SerializeField] private Animator popUpAnimator;

    private void Start()
    {

        buttons[0].onClick.AddListener(delegate { OpenMemory(0); });
        buttons[1].onClick.AddListener(delegate { OpenMemory(1); });
        buttons[2].onClick.AddListener(delegate { OpenMemory(2); });
        buttons[3].onClick.AddListener(delegate { OpenMemory(3); });
        buttons[4].onClick.AddListener(delegate { OpenMemory(4); });
        buttons[5].onClick.AddListener(delegate { OpenMemory(5); });
    }


    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            memories[0].SetActive(true);
            memories[1].SetActive(true);
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2))
        {
            memories[0].SetActive(true);
            memories[1].SetActive(true);
            memories[2].SetActive(true);
            memories[3].SetActive(true);
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3))
        {
            memories[0].SetActive(true);
            memories[1].SetActive(true);
            memories[2].SetActive(true);
            memories[3].SetActive(true);
            memories[4].SetActive(true);
            memories[5].SetActive(true);
        }


        if (interactionStatus.HasInteracted(memories[0].name))
        {
            memories[0].transform.GetChild(1).gameObject.SetActive(true);
        }
        if (interactionStatus.HasInteracted(memories[1].name))
        {
            memories[1].transform.GetChild(1).gameObject.SetActive(true);
        }
        if (interactionStatus.HasInteracted(memories[2].name))
        {
            memories[2].transform.GetChild(1).gameObject.SetActive(true);
        }
        if (interactionStatus.HasInteracted(memories[3].name))
        {
            memories[3].transform.GetChild(1).gameObject.SetActive(true);
        }
        if (interactionStatus.HasInteracted(memories[4].name))
        {
            memories[4].transform.GetChild(1).gameObject.SetActive(true);
        }
        if (interactionStatus.HasInteracted(memories[5].name))
        {
            memories[5].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void OpenPopUp(bool _corrupted)
    {
        if (_corrupted)
        {
            popUpImage.sprite = corruptedImage;
            popUpText.text = "File corrupted!";
        }
        popUpAnimator.SetBool("OpenPopUp", true);
    }

    public void ClosePopUp()
    {
        DialogueManager.isActive = false;
        popUpAnimator.SetBool("OpenPopUp", false);
    }

    public void OpenMemory(int _value)
    {
        memoryImage.sprite = memoryAsset.memorySprites[_value];
        memoryText.text = memoryAsset.infoTexts[_value];
        journalAnim.SetBool("IsMemoryOpen", true);
        panel.SetActive(true);
    }

    public void CloseMemory()
    {
        journalAnim.SetBool("IsMemoryOpen", false);
        panel.SetActive(false);
    }
}
