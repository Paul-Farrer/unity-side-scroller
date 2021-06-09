using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] private float timeDelay = 1f;
    [SerializeField] private AudioClip shipCrash;
    [SerializeField] private AudioClip shipCompletion;

    [SerializeField] private ParticleSystem shipCrashParticles;
    [SerializeField] private ParticleSystem shipCompletionParticles;

    private AudioSource audioSource;
    private bool isTransitioning = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // caches the reference component AudioSource
    }

    private void Update()
    {
        Debug();
    }

    private void Debug()
    {
        // Cheat keys for the developer
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isTransitioning)
            {
                isTransitioning = false;
            }
            else if (!isTransitioning)
            {
                isTransitioning = true;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if isTransitioning is true non of the code is run below
        if (isTransitioning)
        {
            return;
        }

        // depending upon the case a different method will be called
        // a case depends upon the object collided with the tags specified
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartCrashSequence()
    {
        isTransitioning = true; // sets isTransitioning to true
        shipCrashParticles.Play(); // plays particle effects
        audioSource.Stop(); // stops audio from being played
        audioSource.PlayOneShot(shipCrash); // plays referenced audio
        GetComponent<Movement>().enabled = false; // disables the ships movement by disabling the movement component
        Invoke("ReloadLevel", timeDelay); // invokes method ReloadLevel() with a specified delay
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        shipCompletionParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(shipCompletion);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", timeDelay);
    }

    private void LoadNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex; // returns the idex of the scene in the build settings
        int nextSceneIndex = currentLevelIndex + 1; // adds 1 to the idex allowing the next level to load
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) // if the scene is equal to the total number of scenes then it is set to 0 (Restarting the game)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex); // loads the scene depending upon nextSceneIndex variable
    }

    private void ReloadLevel()
    {
        // reloads the current scene
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
    }
}
