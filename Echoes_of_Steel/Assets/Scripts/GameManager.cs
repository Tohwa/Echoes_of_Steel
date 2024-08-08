using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("Singleton")]
    public static GameManager Instance;

    [Header("Integer Variables")]
    public int consciousMeter = 0;
    public int corruptionMeter = 0;
    public int consciousThreshold = 100;
    public int corruptionThreshold = 100;

    public bool gamePaused = false;
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (gamePaused || DialogueManager.isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        
    }
}
