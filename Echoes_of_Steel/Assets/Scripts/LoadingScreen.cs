using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class LoadingScreen : MonoBehaviour
{
    #region Variables

    [Header("GameObject Variables")]
    [SerializeField] private GameObject loadingScreen;

    [Header("Image Variables")]
    [SerializeField] private Image progressBar;

    [Header("TMP Variables")]
    [SerializeField] private TMP_Text progressText;
    
    [Header("float Variables")]
    [SerializeField] private float artificialLoadingDuration = 3f;

    #endregion

    void Start()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("UI");

        loadingScreen = canvas.transform.GetChild(2).gameObject;
        progressBar = loadingScreen.transform.GetChild(0).gameObject.GetComponent<Image>();
        progressText = loadingScreen.transform.GetChild(2).gameObject.GetComponent<TMP_Text>();   
    }

    public void LoadScene(int _sceneId)
    {
        StartCoroutine(LoadSceneAsync(_sceneId));
    }

    IEnumerator LoadSceneAsync(int _sceneId)
    {
        loadingScreen.SetActive(true);
        float loadStartTime = Time.time;

        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneId);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            UpdateLoadingUI(progress);

            if (operation.progress >= 0.9f)
            {
                break;
            }

            yield return null;
        }

        float actualLoadTime = Time.time - loadStartTime;

        if (actualLoadTime < artificialLoadingDuration)
        {
            float remainingTime = artificialLoadingDuration - actualLoadTime;

            for (float t = 0; t < remainingTime; t += Time.deltaTime)
            {
                float progress = Mathf.Clamp01((actualLoadTime + t) / artificialLoadingDuration);
                UpdateLoadingUI(progress);
                yield return null;
            }
        }

        operation.allowSceneActivation = true;
    }

    void UpdateLoadingUI(float progress)
    {
        progressBar.fillAmount = progress;
        progressText.text = (progress * 100f).ToString("F2") + "%";
    }
}