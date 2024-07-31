using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameEndingHandler : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private LoadingScreen loadScreen;
    void Start()
    {
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        if(loadScreen == null)
        {
            loadScreen = GameObject.FindObjectOfType<LoadingScreen>();
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
            gameObject.SetActive(false);

            loadScreen.LoadScene(0);
        }
    }
}
