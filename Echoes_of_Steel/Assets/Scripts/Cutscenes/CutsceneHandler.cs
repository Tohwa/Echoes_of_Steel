using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutscenesHandler : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private LoadingScreen loadScreen;
    void Start()
    {
        if (playableDirector == null)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        if (loadScreen == null)
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
        Scene scene = SceneManager.GetActiveScene();

        if (director == playableDirector)
        {
            if (scene.buildIndex == 1)
            {
                GameObject.FindGameObjectWithTag("Player").gameObject.transform.position = new Vector3(223.01f, -85.18f, 1293.9f);

                gameObject.transform.parent.gameObject.SetActive(false);
            }
            else if (scene.buildIndex == 3)
            {
                gameObject.transform.parent.gameObject.SetActive(false);

                GameManager.Instance.gamePaused = true;
                loadScreen.LoadScene(0);

                SteamUserStats.ResetAllStats(true);
                Debug.Log("Steam Stats wurden gelöscht.");
            }
        }
    }
}
