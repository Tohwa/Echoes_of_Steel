using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DirectorDisabler : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;

    void Start()
    {
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        if (playableDirector != null)
        {
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
    }

    void Update()
    {
        if (playableDirector != null && playableDirector.time >= playableDirector.duration)
        {
            OnPlayableDirectorStopped(playableDirector);
        }
    }

    void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            GameObject.FindGameObjectWithTag("Player").gameObject.transform.position = new Vector3(223.01f, 57.09f, 1293.9f);
           
            gameObject.SetActive(false);

        }
    }
}
