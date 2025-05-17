using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Загружаем игровую сцену по индексу (1)
        SceneManager.LoadScene(1);
        
        // Или по имени (если хотите)
        // SceneManager.LoadScene("GameScene");
    }
}