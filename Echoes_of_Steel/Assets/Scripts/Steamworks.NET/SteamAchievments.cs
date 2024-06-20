using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SteamAchievment : MonoBehaviour
{
    void Start()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }
    }

    private void UpdateSteamStats()
    {
        SteamUserStats.StoreStats();
    }
}
