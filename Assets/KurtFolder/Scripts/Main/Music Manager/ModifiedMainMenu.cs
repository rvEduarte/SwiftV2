using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModifiedMainMenu : MonoBehaviour
{
    private static ModifiedMainMenu backgroundMusic;

    void Awake()
    {
        // If no instance exists, set this as the background music
        if (backgroundMusic == null)
        {
            backgroundMusic = this;
            DontDestroyOnLoad(backgroundMusic); // Keep the music object active across scenes
        }
        else
        {
            Destroy(gameObject); // If an instance already exists, destroy this one
        }
    }

    private void OnEnable()
    {
        // Subscribe to the scene load event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the scene load event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when a new scene is loaded
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        StopMusic();
    }

    private void StopMusic()
    {
        if (backgroundMusic != null)
        {
            // Stop playing the music and destroy the GameObject
            AudioSource audioSource = backgroundMusic.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Stop();
            }

            Destroy(backgroundMusic.gameObject); // Destroy the background music GameObject
            backgroundMusic = null;
        }
    }
}
