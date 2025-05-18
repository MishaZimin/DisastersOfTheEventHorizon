using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour 
{
    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text survivalTimeText;

    [Header("UI Canvases")]
    [SerializeField] private Canvas topCanvas; 
    [SerializeField] private Canvas bottomCanvas; 
    [SerializeField] private Canvas shopCanvas;

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;

    private bool gameEnded = false;
    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        SetGameUIState(active: true);
    }

    void Update()
    {
        if (gameEnded) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameEnded = true;
        Debug.Log("Game Over!");

        SetGameUIState(active: false);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void SetGameUIState(bool active)
    {
        if (topCanvas != null)
            topCanvas.enabled = active;

        if (bottomCanvas != null)
            bottomCanvas.enabled = active;

        if (shopCanvas != null)
            shopCanvas.enabled = active;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        gameEnded = false;
        SetGameUIState(active: true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel?.SetActive(true);
        SetGameUIState(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel?.SetActive(false);
        SetGameUIState(true);
    }
}