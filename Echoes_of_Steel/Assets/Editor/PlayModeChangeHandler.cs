using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PlayModeChangeHandler : MonoBehaviour
{
    static PlayModeChangeHandler()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            DeleteSteamStats();
        }
    }

    private static void DeleteSteamStats()
    {
        SteamUserStats.ResetAllStats(true);
        Debug.Log("Steam Stats wurden gelöscht.");
    }
}
