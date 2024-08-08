using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameUIHandler : MonoBehaviour
{
    [Header("Memory Objects")]
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private MemoryAsset memoryAsset;
    [SerializeField] private GameObject[] memories;
    [SerializeField] private Button[] buttons;
    [SerializeField] private TextMeshProUGUI memoryText;
    [SerializeField] private Image memoryImage;
    [SerializeField] private GameObject panelMemory;
    [SerializeField] private GameObject panelJournal;
    [SerializeField] private Animator journalAnimator;

    [Header("Pop-up Objects")]
    [SerializeField] private TextMeshProUGUI popUpText;
    [SerializeField] private GameObject corruptedImage;
    [SerializeField] private VideoPlayer shieldVideo;
    [SerializeField] private VideoPlayer weaponVideo;
    [SerializeField] private Animator popUpAnimator;

    [Header("Pause menu Objects")]
    [SerializeField] private GameObject pauseMenu;

    private bool journalIsOpen;

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
        // Memories will get enabled with each scene
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

        // Checks if memory was collected
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

    public void OpenJournal()
    {
        if (!journalIsOpen)
        {
            journalAnimator.SetBool("IsOpen", true);
            panelJournal.SetActive(true);
            journalIsOpen = true;
            GameManager.Instance.gamePaused = true;

        }
        else
        {
            journalAnimator.SetBool("IsOpen", false);
            panelJournal.SetActive(false);
            journalIsOpen = false;
            GameManager.Instance.gamePaused = false;

        }
    }

    public void OpenPopUp(bool _corrupted)
    {
        if (_corrupted)
        {
            weaponVideo.gameObject.SetActive(false);
            corruptedImage.SetActive(true);
            popUpText.text = "File corrupted!";
        }
        else
        {
            weaponVideo.Play();
        }
        shieldVideo.Play();
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
        journalAnimator.SetBool("IsMemoryOpen", true);
        panelMemory.SetActive(true);
    }

    public void CloseMemory()
    {
        journalAnimator.SetBool("IsMemoryOpen", false);
        panelMemory.SetActive(false);
    }
    public void PauseGame()
    {
        if (!GameManager.Instance.gamePaused)
        {
            pauseMenu.SetActive(true);
            GameManager.Instance.gamePaused = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            GameManager.Instance.gamePaused = false;
        }
    }
}
