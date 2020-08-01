using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUi : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f; //TimeFreeze set
    }

    public void Resume()
    {
        gameObject.SetActive(false); //mit dem kleinen gameObject frage ich mich selbst ab.
        print("Resume");
        Time.timeScale = 1f; //TimeFreeze reset
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Hauptmenü()
    {
        SceneManager.LoadScene(0);
    }
}
