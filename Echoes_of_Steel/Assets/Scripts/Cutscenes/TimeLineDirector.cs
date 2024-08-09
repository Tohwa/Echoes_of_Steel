using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineDirector : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject cameraObject;

    [SerializeField] private GameObject endingOne;
    [SerializeField] private GameObject endingTwo;
    [SerializeField] private GameObject endingThree;
    [SerializeField] private GameObject endingFour;
    [SerializeField] private GameObject hologramObject;

    [SerializeField] private PlayableDirector endingOneDirector;
    [SerializeField] private PlayableDirector endingTwoDirector;
    [SerializeField] private PlayableDirector endingThreeDirector;
    [SerializeField] private PlayableDirector endingFourDirector;

    private void CheckCorruptionMeter()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        cameraObject = FindObjectOfType<CameraController>().gameObject;

        if (GameManager.Instance.consciousMeter < GameManager.Instance.consciousThreshold && GameManager.Instance.corruptionMeter < GameManager.Instance.corruptionThreshold && endingOneDirector.state != PlayState.Playing)
        {
            playerObject.SetActive(false);
            cameraObject.SetActive(false);
            hologramObject.SetActive(false);

            endingOne.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(endingOneDirector);

            SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME");
        }
        else if (GameManager.Instance.consciousMeter < GameManager.Instance.consciousThreshold && GameManager.Instance.corruptionMeter >= GameManager.Instance.corruptionThreshold && endingTwoDirector.state != PlayState.Playing)
        {
            playerObject.SetActive(false);
            cameraObject.SetActive(false);
            hologramObject.SetActive(false);

            endingTwo.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(endingTwoDirector);

            SteamUserStats.SetAchievement("ACH_WIN_100_GAMES");
        }
        else if (GameManager.Instance.consciousMeter >= GameManager.Instance.consciousThreshold && GameManager.Instance.corruptionMeter < GameManager.Instance.corruptionThreshold && endingThreeDirector.state != PlayState.Playing)
        {
            playerObject.SetActive(false);
            cameraObject.SetActive(false);
            hologramObject.SetActive(false);

            endingThree.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(endingThreeDirector);

            SteamUserStats.SetAchievement("ACH_TRAVEL_FAR_SINGLE");
        }
        else if (GameManager.Instance.consciousMeter >= GameManager.Instance.consciousThreshold && GameManager.Instance.corruptionMeter >= GameManager.Instance.corruptionThreshold && endingFourDirector.state != PlayState.Playing)
        {
            playerObject.SetActive(false);
            cameraObject.SetActive(false);
            hologramObject.SetActive(false);

            endingFour.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(endingFourDirector);

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
