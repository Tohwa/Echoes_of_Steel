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
    public int corruptionMeter = 0;

    [Header("boolean Variables")]
    public bool endingOne = false;
    public bool endingTwo = false;
    public bool endingThree = false;
    public bool endingFour = false;
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
