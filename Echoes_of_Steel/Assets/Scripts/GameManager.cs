using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int corruptionMeter = 0;

    public bool endingOne = false;
    public bool endingTwo = false;
    public bool endingThree = false;
    public bool endingFour = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
