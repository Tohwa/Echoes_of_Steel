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

    [Header("Image Variables")]
    [SerializeField] private Image progressBar;

    [Header("TMP Variables")]
    [SerializeField] private TMP_Text progressText;

    [Header("float Variables")]
    [SerializeField] private float artificialLoadingDuration = 3f;

    #endregion

    void Start()
    {
        if (loadingScreen == null)
        {
            loadingScreen = transform.Find("LoadingScreen").gameObject;
        }

        if (loadingScreen != null)
        {
            progressBar = loadingScreen.GetComponentInChildren<Image>();
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
        if (progressBar != null && progressText != null)
        {
            progressBar.fillAmount = progress;
            progressText.text = (progress * 100f).ToString("F2") + "%";
            Debug.Log(progressBar.fillAmount);
        }
        else
        {
            Debug.LogWarning("ProgressBar or ProgressText is not assigned.");
        }
    }
}