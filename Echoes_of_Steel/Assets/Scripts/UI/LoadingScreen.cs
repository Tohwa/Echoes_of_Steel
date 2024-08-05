using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Image = UnityEngine.UI.Image;

public class LoadingScreen : MonoBehaviour
{
    #region Variables

    [Header("GameObject Variables")]
    private GameObject loadingScreen;
    private GameObject progressBarObj;

    [Header("Image Variables")]
    [SerializeField] private Image progressBar;

    [Header("TMP Variables")]
    [SerializeField] private TMP_Text progressText;

    [Header("float Variables")]
    [SerializeField] private float artificialLoadingDuration = 3f;

    [Header("bool Variables")]
    private bool isLoading = false;

    #endregion

    void Start()
    {
        if (loadingScreen == null)
        {
            loadingScreen = transform.Find("LoadingScreen").gameObject;

        }

        if (loadingScreen != null)
        {
            progressBarObj = loadingScreen.transform.GetChild(0).gameObject;
            progressBar = progressBarObj.GetComponent<Image>();
            progressText = loadingScreen.GetComponentInChildren<TMP_Text>();
        }
        else
        {
            if (progressBar == null)
            {
                Debug.LogError("Image Component could not be found!");
            }

            if (progressText == null)
            {
                Debug.LogError("TMP_Text Component could not be found!");
            }
        }
    }

    public void LoadScene(int _sceneId)
    {
        if(!isLoading)
        {
            Debug.Log("Loading scene with ID: " + _sceneId);
            StartCoroutine(LoadSceneAsync(_sceneId));
        }
        else
        {
            Debug.LogWarning("Already loading a scene. New request ignored.");
        }
    }

    IEnumerator LoadSceneAsync(int _sceneId)
    {
        isLoading = true;
        loadingScreen.SetActive(true);
        float loadStartTime = Time.time;

        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneId);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            UpdateLoadingUI(progress);

            Debug.Log("Loading progress: " + progress);

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

        Debug.Log("Loading complete, activating scene.");
        operation.allowSceneActivation = true;
        isLoading = false;
    }

    void UpdateLoadingUI(float progress)
    {
        if (progressBar != null && progressText != null)
        {
            progressBar.fillAmount = progress;
            progressText.text = (progress * 100f).ToString("F2") + "%";
            Debug.Log("ProgressBar fill amount: " + progressBar.fillAmount);
        }
        else
        {
            Debug.LogWarning("ProgressBar or ProgressText is not assigned.");
        }
    }
}