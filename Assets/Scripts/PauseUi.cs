using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f; //TimeFreeze set
    }

    public void Resume()
    {
        gameObject.SetActive(false); //mit dem kleinen gameObject frage ich mich selbst ab.
        Time.timeScale = 1f; //TimeFreeze reset
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Hauptmenü()
    {
        SceneManager.LoadScene(0);
    }
}
