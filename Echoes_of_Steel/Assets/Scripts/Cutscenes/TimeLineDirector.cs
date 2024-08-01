using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineDirector : MonoBehaviour
{
    [SerializeField] private GameObject m_Player;
    [SerializeField] private GameObject m_Camera;

    [SerializeField] private GameObject m_goodEnding;
    [SerializeField] private GameObject m_badEnding;

    [SerializeField] private PlayableDirector m_goodDirector;
    [SerializeField] private PlayableDirector m_badDirector;

    private void CheckCorruptionMeter()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Camera = FindObjectOfType<CameraController>().gameObject;

        if (GameManager.Instance.corruptionMeter >= 0 && GameManager.Instance.corruptionMeter < 15 && m_goodDirector.state != PlayState.Playing)
        {
            m_Player.SetActive(false);
            m_Camera.SetActive(false);

            m_goodEnding.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(m_goodDirector);

            SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME");
        }
        else if (GameManager.Instance.corruptionMeter >= 15 && GameManager.Instance.corruptionMeter < 30 && m_goodDirector.state != PlayState.Playing)
        {
            m_Player.SetActive(false);
            m_Camera.SetActive(false);

            m_goodEnding.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(m_goodDirector);

            SteamUserStats.SetAchievement("ACH_WIN_100_GAMES");
        }
        else if (GameManager.Instance.corruptionMeter >= 30 && GameManager.Instance.corruptionMeter < 45 && m_badDirector.state != PlayState.Playing)
        {
            m_Player.SetActive(false);
            m_Camera.SetActive(false);

            m_badEnding.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(m_badDirector);

            SteamUserStats.SetAchievement("ACH_TRAVEL_FAR_SINGLE");
        }
        else if (GameManager.Instance.corruptionMeter >= 45 && m_badDirector.state != PlayState.Playing)
        {
            m_Player.SetActive(false);
            m_Camera.SetActive(false);

            m_badEnding.gameObject.transform.parent.gameObject.SetActive(true);

            PlayTimeLine(m_badDirector);

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
