using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineDirector : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject cameraObject;

    [SerializeField] private GameObject goodEnding;
    [SerializeField] private GameObject badEnding;
    [SerializeField] private GameObject hologramObject;

    [SerializeField] private PlayableDirector goodDirector;
    [SerializeField] private PlayableDirector badDirector;

    private void CheckCorruptionMeter()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        cameraObject = FindObjectOfType<CameraController>().gameObject;

        if (GameManager.Instance.corruptionMeter >= 0 && GameManager.Instance.corruptionMeter < 15 && goodDirector.state != PlayState.Playing)
        {
            playerObject.SetActive(false);
            cameraObject.SetActive(false);
            hologramObject.SetActive(false);

            goodEnding.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(goodDirector);

            SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME");
        }
        else if (GameManager.Instance.corruptionMeter >= 15 && GameManager.Instance.corruptionMeter < 30 && goodDirector.state != PlayState.Playing)
        {
            playerObject.SetActive(false);
            cameraObject.SetActive(false);
            hologramObject.SetActive(false);

            goodEnding.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(goodDirector);

            SteamUserStats.SetAchievement("ACH_WIN_100_GAMES");
        }
        else if (GameManager.Instance.corruptionMeter >= 30 && GameManager.Instance.corruptionMeter < 45 && badDirector.state != PlayState.Playing)
        {
            playerObject.SetActive(false);
            cameraObject.SetActive(false);
            hologramObject.SetActive(false);

            badEnding.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(badDirector);

            SteamUserStats.SetAchievement("ACH_TRAVEL_FAR_SINGLE");
        }
        else if (GameManager.Instance.corruptionMeter >= 45 && badDirector.state != PlayState.Playing)
        {
            playerObject.SetActive(false);
            cameraObject.SetActive(false);
            hologramObject.SetActive(false);

            badEnding.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(badDirector);

            SteamUserStats.SetAchievement("ACH_TRAVEL_FAR_ACCUM");
        }

        UpdateSteamStats();
    }

    private void PlayTimeLine(PlayableDirector _director)
    {
        _director.Play();
    }

    private void UpdateSteamStats()
    {
        SteamUserStats.StoreStats();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
                CheckCorruptionMeter();  
        }
    }
}
