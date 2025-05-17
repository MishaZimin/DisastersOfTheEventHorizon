using UnityEngine;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour
{

    public Transform enemyPrefab;
    public Transform bossPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public TextMeshProUGUI waveCountdownText;

    private int waveIndex = 0;
    public static int CurrentWave = 0;

    public TextMeshProUGUI gameTimerText;
    private float gameTime = 0f;

    public static float TotalGameTime { get; private set; }


    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);

        gameTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);

        gameTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        TotalGameTime = gameTime;
    }

    IEnumerator SpawnWave()
    {
        CurrentWave = ++waveIndex;

        if (waveIndex % 3 == 0)
        {
            SpawnBoss();
        }
        else
        {
            int enemiesCount = Mathf.FloorToInt(3 + waveIndex * 1.15f);
            for (int i = 0; i < enemiesCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(Random.Range(0.2f, 0.6f));
            }
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    void SpawnBoss()
    {
        Transform boss = Instantiate(bossPrefab, spawnPoint.position + new Vector3(0f, 2f, 0f), spawnPoint.rotation);
        Enemy enemyScript = boss.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.isBoss = true;
        }
    }

public float GetCurrentGameTime()
{
    return gameTime;
}
}