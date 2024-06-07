using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;

    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider volumeSlider;

    private Resolution[] resolutions;
    private List<Resolution> compatibleResolutions = new List<Resolution>();
    private bool resolutionSet;
    private int resolutionIndex;
    private int qualityIndex;
    private bool toggleFullscreen = true;
    private float volumeValue = 1f;

    private void Start()
    {
        resolutions = Screen.resolutions;

        List<string> resOptions = new List<string>();
        int curResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resOption = resolutions[i].width + "x" + resolutions[i].height;
            if(resOption == "1280x720" || resOption == "1600x900" || resOption == "1920x1080")
            {
                compatibleResolutions.Add(resolutions[i]);
                resOptions.Add(resOption);
            }

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                curResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(resOptions);
        if (!resolutionSet)
        {
            resolutionIndex = curResolutionIndex;
            resolutionDropdown.value = resolutionIndex;
            resolutionSet = true;
        }
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        //resolutionDropdown.value = resolutionIndex;
        //qualityDropdown.value = qualityIndex;
        //fullscreenToggle.isOn = toggleFullscreen;
        //volumeSlider.value = volumeValue;
    }

    public void SetMasterVolume(float _volume)
    {
        mainMixer.SetFloat("masterVolume", Mathf.Log10(_volume) * 20);
        volumeValue = _volume;
    }

    public void SetMusicVolume(float _volume)
    {
        mainMixer.SetFloat("musicVolume", Mathf.Log10(_volume) * 20);
        volumeValue = _volume;
    }

    public void SetSFXVolume(float _volume)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(_volume) * 20);
        volumeValue = _volume;
    }

    public void SetUIVolume(float _volume)
    {
        mainMixer.SetFloat("UIVolume", Mathf.Log10(_volume) * 20);
        volumeValue = _volume;
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

    public void BackToMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

}
