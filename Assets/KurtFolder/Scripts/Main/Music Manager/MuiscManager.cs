using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    // Define which audio clip to play in each scene
    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;       // Scene name
        public AudioClip musicClip;    // Music clip to play
    }

    // List of scenes and their corresponding music
    [SerializeField] private List<SceneMusic> sceneMusicList = new List<SceneMusic>();

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

        // Look for the appropriate music clip for the loaded scene
        foreach (SceneMusic sceneMusic in sceneMusicList)
        {
            if (currentScene == sceneMusic.sceneName)
            {
                PlayMusic(sceneMusic.musicClip);
                return;
            }
        }

        // If no specific music found for this scene, stop the music
        StopMusic();
    }

    // Function to play music
    private void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.Stop();           // Stop the current music
            audioSource.clip = clip;      // Assign the new clip
            audioSource.Play();           // Play the new clip
        }
    }

    // Function to stop music
    private void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;      // Clear the clip
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
