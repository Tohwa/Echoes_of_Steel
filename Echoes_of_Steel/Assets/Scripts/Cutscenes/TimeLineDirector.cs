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

    [SerializeField] private float m_waitDureation = 5f;

    private void Start()
    {
        m_goodEnding = gameObject.transform.GetChild(0).gameObject;
        m_badEnding = gameObject.transform.GetChild(1).gameObject;
    }

    private void CheckCorruptionMeter()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Camera = FindObjectOfType<CameraController>().gameObject;

        if (GameManager.Instance.corruptionMeter >= 0 && GameManager.Instance.corruptionMeter < 20 && m_goodDirector.state != PlayState.Playing)
        {
            m_Player.SetActive(false);
            m_Camera.SetActive(false);

            m_goodEnding.SetActive(true);

            PlayTimeLine(m_goodDirector);
        }
        else if (GameManager.Instance.corruptionMeter >= 20 && GameManager.Instance.corruptionMeter < 40 && m_goodDirector.state != PlayState.Playing)
        {
            m_Player.SetActive(false);
            m_Camera.SetActive(false);

            m_goodEnding.SetActive(true);

            PlayTimeLine(m_goodDirector);
        }
        else if (GameManager.Instance.corruptionMeter >= 40 && GameManager.Instance.corruptionMeter < 60 && m_badDirector.state != PlayState.Playing)
        {
            m_Player.SetActive(false);
            m_Camera.SetActive(false);

            m_badEnding.SetActive(true);

            PlayTimeLine(m_badDirector);
        }
        else if (GameManager.Instance.corruptionMeter >= 60 && GameManager.Instance.corruptionMeter < 80 && m_badDirector.state != PlayState.Playing)
        {
            m_Player.SetActive(false);
            m_Camera.SetActive(false);

            m_badEnding.SetActive(true);

            PlayTimeLine(m_badDirector);
        }
    }

    private void PlayTimeLine(PlayableDirector _director)
    {
        _director.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_waitDureation -= Time.deltaTime;
            
            if(m_waitDureation <= 0)
            {
                m_waitDureation = 0;

                CheckCorruptionMeter();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (m_waitDureation < 0)
            {
                m_waitDureation = 5f;
            }
        }
    }


}
