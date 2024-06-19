using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Scene scene = SceneManager.GetActiveScene();

        if (other.gameObject.CompareTag("Player"))
        {
            if (scene.buildIndex == 1)
            {
                SceneManager.LoadScene("Level Two");
            }
            else if (scene.buildIndex == 2)
            {
                SceneManager.LoadScene("Level Three");
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

                SceneManager.LoadScene("EndingScreen");
            }
        }
    }
}
