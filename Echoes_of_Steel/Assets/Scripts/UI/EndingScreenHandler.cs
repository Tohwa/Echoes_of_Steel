using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScreenHandler : MonoBehaviour
{
    [SerializeField] private GameObject firstEnding;
    [SerializeField] private GameObject secondEnding;
    [SerializeField] private GameObject thirdEnding;
    [SerializeField] private GameObject fourthEnding;

    [SerializeField] private LoadingScreen loadingScreen;

    private void Start()
    {
        GameManager.Instance.gamePaused = true;

        if (GameManager.Instance.endingOne)
        {
            firstEnding.SetActive(true);
            SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME");
        }
        else if(GameManager.Instance.endingTwo)
        {
            secondEnding.SetActive(true);
            SteamUserStats.SetAchievement("ACH_WIN_100_GAMES");

        }
        else if(GameManager.Instance.endingThree)
        {
            thirdEnding.SetActive(true);
            SteamUserStats.SetAchievement("ACH_TRAVEL_FAR_SINGLE");

        }
        else if (GameManager.Instance.endingFour)
        {
            fourthEnding.SetActive(true);
            SteamUserStats.SetAchievement("ACH_TRAVEL_FAR_ACCUM");
        }

        UpdateSteamStats();
    }

    public void ReturnToMainMenu()
    {
        loadingScreen.LoadScene(0);
    }

    private void UpdateSteamStats()
    {
        SteamUserStats.StoreStats();
    }
}
