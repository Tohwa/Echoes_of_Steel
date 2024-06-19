using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadScreen;

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
            else if(scene.buildIndex == 3)
            {
                if(GameManager.Instance.corruptionMeter >= -40 && GameManager.Instance.corruptionMeter < -20)
                {
                    GameManager.Instance.endingOne = true;
                }
                else if (GameManager.Instance.corruptionMeter >= -20 && GameManager.Instance.corruptionMeter < 0)
                {
                    GameManager.Instance.endingTwo = true;
                }
                else if (GameManager.Instance.corruptionMeter >= 0 && GameManager.Instance.corruptionMeter < 20)
                {
                    GameManager.Instance.endingThree = true;
                }
                else if (GameManager.Instance.corruptionMeter >= 20 && GameManager.Instance.corruptionMeter < 40)
                {
                    GameManager.Instance.endingFour = true;
                }

                loadScreen.LoadScene(4); ;
            }
        }
    }
}
