using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnButtonExitClick()
    {
        Application.Quit();
    }

    public void OnButtonNewGameClick()
    {
        SceneManager.LoadScene("Level1");
    }
}
