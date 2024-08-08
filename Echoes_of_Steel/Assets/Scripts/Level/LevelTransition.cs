using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadScreen;

    private void Awake()
    {
        
    }

    private void Start()
    {
        loadScreen = GameObject.FindObjectOfType<LoadingScreen>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Scene scene = SceneManager.GetActiveScene();

        if (other.gameObject.CompareTag("Player"))
        {
            if (scene.buildIndex == 1)
            {
                loadScreen.LoadScene(2);
            }
            else if (scene.buildIndex == 2)
            {
                loadScreen.LoadScene(3);
            }
        }
    }
}
