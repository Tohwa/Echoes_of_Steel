using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;

    [SerializeField] private AudioMixer mainMixer;

    [SerializeField] private TextMeshProUGUI resText;
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider[] volumeSlider;

    private List<string> resOptions;

    private Resolution[] resolutions;
    private List<Resolution> compatibleResolutions = new List<Resolution>();
    private string[] qualities = {"Very Low", "Low", "Medium", "High", "Very High", "Ultra"};
    //private bool resolutionSet;
    //private int resolutionIndex;
    //private int qualityIndex = 0;
    //private bool toggleFullscreen = true;

    private static bool resolutionSet;
    private static int resolutionIndex;
    private static int qualityIndex = 5;
    private static bool toggleFullscreen = true;
    private static float masterVolume = 1f;
    private static float musicVolume = 1f;
    private static float SFXVolume = 1f;
    private static float UIVolume = 1f;


    private void Start()
    {
        //volumeSlider.onValueChanged.AddListener(delegate { SetMasterVolume(volumeValue); });

        resolutions = Screen.resolutions;

        resOptions = new List<string>();
        int curResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resOption = resolutions[i].width + "x" + resolutions[i].height;
            if(resOption == "1280x720" || resOption == "1600x900" || resOption == "1920x1080")
            {
                compatibleResolutions.Add(resolutions[i]);
                resOptions.Add(resOption);
            }


            
        }
        for (int i = 0; i < compatibleResolutions.Count; i++)
        {
            if (compatibleResolutions[i].width == Screen.currentResolution.width && compatibleResolutions[i].height == Screen.currentResolution.height)
            {
                curResolutionIndex = i;
            }
        }
        if (!resolutionSet)
        {
            resolutionIndex = curResolutionIndex;
            resolutionSet = true;
        }

        UpdateUI();
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

    public void ResValueUp()
    {
        if(resolutionIndex < compatibleResolutions.Count - 1)
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
        if(qualityIndex < 6)
        {
            qualityIndex++;
            qualityText.text = qualities[qualityIndex];
        }
    }

    public void QualityValueDown()
    {
        if(qualityIndex > 0)
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

    public void UpdateUI()
    {
        resText.text = resOptions[resolutionIndex];
        qualityText.text = qualities[qualityIndex];
        fullscreenToggle.isOn = toggleFullscreen;
        volumeSlider[0].value = masterVolume;
        volumeSlider[1].value = musicVolume;
        volumeSlider[2].value = SFXVolume;
        volumeSlider[3].value = UIVolume;
    }

    public void BackToMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
