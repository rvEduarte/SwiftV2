using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] public GameObject FinishPanel;
    // Start is called before the first frame update

    public MainMenuScriptRv mainMenu;

    private void Start()
    {
        FinishPanel.SetActive(false);
        Time.timeScale = 1;
        LeanTween.init(1000); // Increase the number of spaces to 800 or more as needed
        LeanTween.scale(FinishPanel, Vector3.zero, 0f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            FinishPanel.SetActive(true);
            Debug.Log("tite");
            LeanTween.scale(FinishPanel, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutQuint).setIgnoreTimeScale(true);
            mainMenu.disablePause = true;
            StartCoroutine(WaitAndPause());
        }
    }
    private IEnumerator WaitAndPause()
    {
        yield return new WaitForSecondsRealtime(1); // Wait for 1 second in real-time
        Time.timeScale = 0; // Pause the game
    }
}
