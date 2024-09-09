using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadedANDExit : MonoBehaviour
{
    // Function to load a scene by name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Function to quit the application
    public void QuitApplication()
    {
        // This will only work in a built application (won't quit the editor)
        Application.Quit();

        // For testing purposes in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
