using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineDirector : MonoBehaviour
{
    [SerializeField] private PlayableDirector m_goodDirector;
    [SerializeField] private PlayableDirector m_badDirector;

    [SerializeField] private float m_waitDureation = 5f;

    private void CheckCorruptionMeter()
    {
        if (GameManager.Instance.corruptionMeter >= 0 && GameManager.Instance.corruptionMeter < 20 && m_goodDirector.state != PlayState.Playing)
        {
            PlayTimeLine(m_goodDirector);

            //play Dialogue
        }
        else if (GameManager.Instance.corruptionMeter >= 20 && GameManager.Instance.corruptionMeter < 40)
        {
            PlayTimeLine(m_goodDirector);

            //play Dialogue
        }
        else if (GameManager.Instance.corruptionMeter >= 49 && GameManager.Instance.corruptionMeter < 60)
        {
            PlayTimeLine(m_badDirector);

            //play Dialogue
        }
        else if (GameManager.Instance.corruptionMeter >= 60 && GameManager.Instance.corruptionMeter < 80)
        {
            PlayTimeLine(m_badDirector);

            //play Dialogue
        }
    }

    private void PlayTimeLine(PlayableDirector _director)
    {
        _director.Play();
    }

    private void OnTriggerEnter(Collider other)
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
