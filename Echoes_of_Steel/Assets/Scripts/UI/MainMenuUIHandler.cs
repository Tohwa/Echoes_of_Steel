using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject credits;

    [SerializeField] private LoadingScreen loadingScreen;

    public void StartGame()
    {
        loadingScreen.LoadScene(1);
        GameManager.Instance = null;
        DialogueManager.instance = null;
    }

    public void OpenOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    private void OnDestroy()
    {
        GameManager.Instance.gamePaused = false;
    }
}
