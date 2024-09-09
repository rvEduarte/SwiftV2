using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MuiscManager : MonoBehaviour
{
    private static MuiscManager instance;

    // Specify the scene names where music should play and stop
    [SerializeField] private string[] playInScenes;  // Scenes where music should continue playing
    [SerializeField] private string[] stopInScenes;  // Scenes where music should stop

    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton Pattern to ensure only one instance of MusicManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

            // Subscribe to the sceneLoaded event to detect when a scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    // This is called every time a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentScene = scene.name;

        // Check if the current scene is in the list of scenes where music should stop
        foreach (string stopScene in stopInScenes)
        {
            if (currentScene == stopScene)
            {
                StopMusic();
                return;
            }
        }

        // Check if the current scene is in the list of scenes where music should continue
        foreach (string playScene in playInScenes)
        {
            if (currentScene == playScene)
            {
                PlayMusic();
                return;
            }
        }
    }

    // Function to play music
    private void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Function to stop music
    private void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
