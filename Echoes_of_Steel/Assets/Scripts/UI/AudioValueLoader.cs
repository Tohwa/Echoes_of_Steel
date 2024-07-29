using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioValueLoader
{
    private float m_masterValue;
    private float m_musicVolume;
    private float m_sfxVolume;
    private float m_uiVolume;

    public float MasterVolume
    {
        get { return m_masterValue; }
        set { m_masterValue = value; }
    }

    public float MusicVolume
    {
        get { return m_musicVolume; }
        set { m_musicVolume = value; }
    }

    public float SFXVolume
    {
        get { return m_sfxVolume; }
        set { m_sfxVolume = value; }
    }

    public float UIVolume
    {
        get { return m_uiVolume; }
        set { m_uiVolume = value; }
    }
}
