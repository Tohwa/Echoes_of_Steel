using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuUIHandler : MonoBehaviour
{
    #region Variables
    [Header("Menu Objects")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject credits;

    [Header("Options menu Objects")]
    [SerializeField] private TextMeshProUGUI resText;
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider[] volumeSlider;
    [SerializeField] private AudioMixer mainMixer;

    // Resolution & quality variables
    private Resolution[] resolutions;
    private List<string> resOptions;
    private List<Resolution> compatibleResolutions = new List<Resolution>();
    private string[] qualities = { "Very Low", "Low", "Medium", "High", "Very High", "Ultra" };

    // Static option variables
    private static bool resolutionSet;
    private static int resolutionIndex;
    private static int qualityIndex = 5;
    private static bool toggleFullscreen = true;
    private static float masterVolume = 1f;
    private static float musicVolume = 1f;
    private static float SFXVolume = 1f;
    private static float UIVolume = 1f;
    #endregion

    private void Start()
    {
        resolutions = Screen.resolutions;

        resOptions = new List<string>();
        int curResolutionIndex = 0;

        // Create list of available resolutions
        for (int i = 0; i < resolutions.Length; i++)
        {
            string resOption = resolutions[i].width + "x" + resolutions[i].height;
            if (resOption == "1280x720" || resOption == "1600x900" || resOption == "1920x1080")
            {
                if (!resOptions.Contains(resOption))
                {
                    compatibleResolutions.Add(resolutions[i]);
                    resOptions.Add(resOption);

                }
            }
        }

        // Search for current resolution
        for (int i = 0; i < compatibleResolutions.Count; i++)
        {
            if (compatibleResolutions[i].width == Screen.currentResolution.width && compatibleResolutions[i].height == Screen.currentResolution.height)
            {
                curResolutionIndex = i;
            }
        }

        // Set resolution, if not already been set
        if (!resolutionSet)
        {
            resolutionIndex = curResolutionIndex;
            resolutionSet = true;
        }

        UpdateUI();
    }
    public void UpdateUI()
    {
        resText.text = resOptions[resolutionIndex];
        qualityText.text = qualities[qualityIndex];
        SetQuality(qualityIndex);
        fullscreenToggle.isOn = toggleFullscreen;
        ToggleFullscreen(toggleFullscreen);
        volumeSlider[0].value = masterVolume;
        volumeSlider[1].value = musicVolume;
        volumeSlider[2].value = SFXVolume;
        volumeSlider[3].value = UIVolume;
    }

    public void SetMasterVolume(float _volume)
    {
        mainMixer.SetFloat("masterVolume", Mathf.Log10(_volume) * 20);
        masterVolume = _volume;
    }

    public void SetMusicVolume(float _volume)
    {
        mainMixer.SetFloat("musicVolume", Mathf.Log10(_volume) * 20);
        musicVolume = _volume;
    }

    public void SetSFXVolume(float _volume)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(_volume) * 20);
        SFXVolume = _volume;
    }

    public void SetUIVolume(float _volume)
    {
        mainMixer.SetFloat("UIVolume", Mathf.Log10(_volume) * 20);
        UIVolume = _volume;
    }

    public void SetQuality(int _quality)
    {
        QualitySettings.SetQualityLevel(_quality);
        qualityIndex = _quality;
    }

    public void SetResolution(int _resolution)
    {
        Screen.SetResolution(compatibleResolutions[_resolution].width, compatibleResolutions[_resolution].height, Screen.fullScreen);
        resolutionIndex = _resolution;
    }

    public void ToggleFullscreen(bool _toggleFullscreen)
    {
        Screen.fullScreen = _toggleFullscreen;
        toggleFullscreen = _toggleFullscreen;
    }

    #region Buttons
    public void ResValueUp()
    {
        if (resolutionIndex < compatibleResolutions.Count - 1)
        {

            resolutionIndex++;
            resText.text = resOptions[resolutionIndex];
        }
    }

    public void ResValueDown()
    {
        if (resolutionIndex > 0)
        {
            resolutionIndex--;
            resText.text = resOptions[resolutionIndex];
        }
    }

    public void QualityValueUp()
    {
        if (qualityIndex < 5)
        {
            qualityIndex++;
            qualityText.text = qualities[qualityIndex];
        }
    }

    public void QualityValueDown()
    {
        if (qualityIndex > 0)
        {
            qualityIndex--;
            qualityText.text = qualities[qualityIndex];
        }
    }

    public void ApplyChanges()
    {
        SetResolution(resolutionIndex);
        SetQuality(qualityIndex);
    }
    public void BackToMenu()
    {
        optionsMenu.SetActive(false);
        credits.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    #endregion


}
