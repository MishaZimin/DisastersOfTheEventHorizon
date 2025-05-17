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
    [SerializeField] private Canvas topCanvas; // Ваш верхний Canvas
    [SerializeField] private Canvas bottomCanvas; // Ваш нижний Canvas
    [SerializeField] private Canvas shopCanvas; // Ваш нижний Canvas
	private WaveSpawner waveSpawner;
    
    private bool gameEnded = false;

    void Start()
    {
		
		Time.timeScale = 1f;

        // Гарантированно скрываем панель при старте
		if (gameOverPanel != null)
			gameOverPanel.SetActive(false);
        
        // Включаем все UI элементы при старте
        SetGameUIState(active: true);
    }

    void Update()
    {
        if (gameEnded) return;

        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }

void EndGame()
{
    gameEnded = true;
    Debug.Log("Game Over!");

    // Показываем, сколько времени продержался игрок
    if (survivalTimeText != null && waveSpawner != null)
{
    float survivedTime = waveSpawner.GetCurrentGameTime();
    int minutes = Mathf.FloorToInt(survivedTime / 60f);
    int seconds = Mathf.FloorToInt(survivedTime % 60f);
    int wave = WaveSpawner.CurrentWave;

    survivalTimeText.text = $"Вы продержались: {minutes:00}:{seconds:00}\nВолна: {wave}";
}
    // Отключаем основные UI элементы
    SetGameUIState(active: false);

    // Включаем панель GameOver
    if (gameOverPanel != null)
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}

	// Управление состоянием UI
	private void SetGameUIState(bool active)
	{
		if (topCanvas != null)
			topCanvas.enabled = active;

		if (bottomCanvas != null)
			bottomCanvas.enabled = active;
			
			if (shopCanvas != null)
            shopCanvas.enabled = active;
    }

    // Исправленный метод рестарта
    public void RestartLevel()
    {

        Time.timeScale = 1f;
        gameEnded = false;
        
        // Включаем UI обратно
        SetGameUIState(active: true);

SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}