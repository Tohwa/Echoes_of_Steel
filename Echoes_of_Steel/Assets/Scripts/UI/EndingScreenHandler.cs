using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScreenHandler : MonoBehaviour
{
    [SerializeField] private GameObject firstEnding;
    [SerializeField] private GameObject secondEnding;
    [SerializeField] private GameObject thirdEnding;
    [SerializeField] private GameObject fourthEnding;

    [SerializeField] private LoadingScreen loadingScreen;

    private void Start()
    {
        GameManager.Instance.gamePaused = true;

        if (GameManager.Instance.endingOne)
        {
            firstEnding.SetActive(true);
        }
        else if(GameManager.Instance.endingTwo)
        {
            secondEnding.SetActive(true);
        }
        else if(GameManager.Instance.endingThree)
        {
            thirdEnding.SetActive(true);
        }
        else if (GameManager.Instance.endingFour)
        {
            fourthEnding.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        loadingScreen.LoadScene(0);
    }
}
