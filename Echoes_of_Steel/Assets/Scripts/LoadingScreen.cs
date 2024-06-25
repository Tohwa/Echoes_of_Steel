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
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text progressText;

    [SerializeField] private bool useArtificialLoadingScreen;
    [SerializeField] private float artificialLoadingDuration = 3f; // Dauer des künstlichen Ladebildschirms in Sekunden

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

        if (useArtificialLoadingScreen)
        {
            yield return StartCoroutine(ArtificialLoadingScreen(_sceneId));
        }
        else
        {
            yield return StartCoroutine(NormalLoadingScreen(_sceneId));
        }
    }

    IEnumerator ArtificialLoadingScreen(int _sceneId)
    {
        float elapsedTime = 0f;

        while (elapsedTime < artificialLoadingDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / artificialLoadingDuration);
            UpdateLoadingUI(progress);

            if(elapsedTime >= artificialLoadingDuration)
            {
                SceneManager.LoadSceneAsync(_sceneId);
            }

            yield return null;
        }
    }

    IEnumerator NormalLoadingScreen(int _sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneId);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            UpdateLoadingUI(progress);
            yield return null;
        }
    }

    void UpdateLoadingUI(float progress)
    {
        progressBar.fillAmount = progress;
        progressText.text = (progress * 100f).ToString("F2") + "%";
    }
}