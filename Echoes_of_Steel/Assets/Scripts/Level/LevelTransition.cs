using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Scene scene = SceneManager.GetActiveScene();

        if (other.gameObject.CompareTag("Player"))
        {
            if (scene.buildIndex == 0)
            {
                SceneManager.LoadScene("Level Two");
            }
            else if (scene.buildIndex == 1)
            {
                SceneManager.LoadScene("Level Three");
            }
            else if(scene.buildIndex == 2)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }
}
