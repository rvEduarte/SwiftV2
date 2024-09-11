using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScriptRv : MonoBehaviour
{
    [SerializeField] public GameObject pauseMenuPanel;
    [SerializeField] public GameObject bg;


    public bool disablePause; //false

    public bool pause; //false

    private void Start()
    {
        Time.timeScale = 1;
        LeanTween.init(1000); // Increase the number of spaces to 800 or more as needed
        LeanTween.scale(pauseMenuPanel, Vector3.zero, 0f);
        pauseMenuPanel.SetActive(false);
        bg.SetActive(false);
    }

    private void Update()
    {
        if (!disablePause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pause = !pause;
                HandlePause();
            }
        }
    }

    private void HandlePause()
    {
        if (pause)
        {
            pauseMenuPanel.SetActive(true);
            Debug.Log("PAUSED");
            LeanTween.scale(pauseMenuPanel, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutQuint).setIgnoreTimeScale(true);
            Time.timeScale = 0;
            bg.SetActive(true);
        }
        else
        {
            LeanTween.scale(pauseMenuPanel, Vector3.zero, 1f).setEase(LeanTweenType.easeOutQuint).setIgnoreTimeScale(true);
            Time.timeScale = 1;
            bg.SetActive(false);
        }
    }

    public void PauseResumeRestart(int id)
    {
        if (id == 0)
        {
            pause = true;
            HandlePause();
        }
        else if (id == 1)
        {
            pause = false;
            HandlePause();
        }
        else
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        }


    }

    public void GoToScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
