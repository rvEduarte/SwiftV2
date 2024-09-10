using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdatedBGMuisc : MonoBehaviour
{
    private static UpdatedBGMuisc instance;

    [SerializeField] private AudioClip[] sceneMusicClips; // Array to store music clips for each scene
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogError("No AudioSource component found on this GameObject. Please add an AudioSource component.");
            }
        }
        else
        {
            Destroy(gameObject); // If an instance already exists, destroy this duplicate
        }
    }

    private void Start()
    {
        PlayMusicForCurrentScene(); // Play the music for the initial scene
        SceneManager.sceneLoaded += OnSceneLoaded; // Listen for scene changes
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene(); // Play the music when the scene changes
    }

    private void PlayMusicForCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneMusicClips != null && sceneMusicClips.Length > sceneIndex && sceneMusicClips[sceneIndex] != null)
        {
            AudioClip newClip = sceneMusicClips[sceneIndex];

            if (audioSource.clip != newClip) // Only change music if it's different
            {
                Debug.Log("Playing music for scene: " + sceneIndex);
                audioSource.clip = newClip;
                audioSource.loop = true;  // Ensure the music loops
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("No audio clip assigned for scene index " + sceneIndex);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe when destroyed
    }
}
