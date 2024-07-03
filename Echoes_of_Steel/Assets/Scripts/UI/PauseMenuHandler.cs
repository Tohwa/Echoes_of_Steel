using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    public GameObject pauseMenu;

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
