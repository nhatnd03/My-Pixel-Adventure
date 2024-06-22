using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public AudioMixer audioMixer;
    public TextMeshProUGUI currentLevel;
    private int currentLevelIndex;
    private void Awake()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        currentLevel.SetText("Level: " + currentLevelIndex.ToString());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        // Time về bình thg
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        // Dừng time của game
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

        Debug.Log("Load menu...");
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
    public void QuitGame()
    {
        Debug.Log("Quit game...");
        Application.Quit();
    }
}
