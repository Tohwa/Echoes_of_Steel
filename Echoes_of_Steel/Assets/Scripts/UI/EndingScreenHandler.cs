using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScreenHandler : MonoBehaviour
{
    #region Variables

    [Header("GameObject Variables")]
    private GameObject firstEnding;
    private GameObject secondEnding;
    private GameObject thirdEnding;
    private GameObject fourthEnding;

    [Header("Script Variables")]
    private LoadingScreen loadingScreen;

    #endregion

    private void Start()
    {
        if(firstEnding == null) 
        {
            firstEnding = transform.Find("EndingOne").gameObject;
        }
        else
        {
            Debug.LogError("First Ending Object could not be found!");
        }

        if (secondEnding == null)
        {
            secondEnding = transform.Find("EndingTwo").gameObject;
        }
        else
        {
            Debug.LogError("Second Ending Object could not be found!");
        }

        if (thirdEnding == null)
        {
            thirdEnding = transform.Find("EndingThree").gameObject;
        }
        else
        {
            Debug.LogError("Third Ending Object could not be found!");
        }

        if (fourthEnding == null)
        {
            fourthEnding = transform.Find("EndingFour").gameObject;
        }
        else
        {
            Debug.LogError("Fourth Ending Object could not be found!");
        }

        if(loadingScreen == null)
        {
            loadingScreen = FindObjectOfType<LoadingScreen>();
        }
        else
        {
            Debug.LogError("loading Screen Script could not be found");
        }

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
