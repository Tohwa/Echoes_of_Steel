using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBarFill;
    [SerializeField] private TMP_Text progressText;

    public void LoadScene(int _sceneId)
    {
        StartCoroutine(LoadSceneAsync(_sceneId));
    }

    IEnumerator LoadSceneAsync(int _sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneId);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            float progressPercentage = progressValue * 100;

            loadingBarFill.fillAmount = progressValue;
            progressText.text = progressPercentage.ToString() + "%";

            yield return null;
        }

        
    }
}
