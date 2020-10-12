using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    private Coroutine coroutine;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Time.timeScale = 0f;

        coroutine = StartCoroutine(Coroutine());
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;

        StopCoroutine(coroutine);
        coroutine = null;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator Coroutine()
    {
        yield return null;
        for (; ; )
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Resume();
                yield break;
            }
            yield return null;
        }
    }
}
