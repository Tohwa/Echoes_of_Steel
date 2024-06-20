using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject pauseMenu;
    public int corruptionMeter = 0;

    public bool endingOne = false;
    public bool endingTwo = false;
    public bool endingThree = false;
    public bool endingFour = false;
    public bool gamePaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (DialogueManager.isActive || gamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void PauseGame()
    {
        if (!gamePaused)
        {
            pauseMenu.SetActive(true);
            gamePaused = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            gamePaused = false;
        }
    }
}
