using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModifiedMainMenu : MonoBehaviour
{
    [SerializeField] public GameObject pauseMenuPanel;
    [SerializeField] public GameObject bg;
    [SerializeField] private Animator animator; // Animator component reference

    public bool disablePause; // false
    public bool pause; // false

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

            // Trigger the animator to play the pause animation
            animator.SetTrigger("LightOn");

            LeanTween.scale(pauseMenuPanel, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutQuint).setIgnoreTimeScale(true);
            Time.timeScale = 0;
            bg.SetActive(true);
        }
        else
        {
            // Trigger the animator to play the close animation
            animator.SetTrigger("LightOff");

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
