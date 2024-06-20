using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SteamAchievment : MonoBehaviour
{
    [SerializeField] InteractionStatus interactionStatus;

    void Start()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }
    }

    private void Update()
    {
        if (interactionStatus.HasInteracted("Memory #1") && interactionStatus.HasInteracted("Memory #2"))
        {
            SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME");
        }

        UpdateSteamStats();

    }

    private void UpdateSteamStats()
    {
        SteamUserStats.StoreStats();
    }
}
