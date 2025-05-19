using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    public int health = 100;
    public int value = 50;

    public GameObject healthBarPrefab;
    private HealthBar healthBar;

    private Transform target;

    private int wavepointIndex = 0;

    private int maxHealth;

    public bool isBoss = false;

    void Start()
    {
        target = Waypoints.points[0];
        int wave = WaveSpawner.CurrentWave;

        if (isBoss)
        {
            speed += Mathf.Max(0.5f, Mathf.Log(wave + 1) * 0.2f);
            health += Mathf.RoundToInt(1000 + Mathf.Pow(wave * 1.5f, 1.5f));
            value += Mathf.RoundToInt(100 + wave * 20);
        }
        else
        {
            speed += Mathf.Log(wave + 1) * 0.3f;
            health += Mathf.RoundToInt(80 + Mathf.Pow(wave, 1.15f));
            value += Mathf.RoundToInt(10 + wave * 3);

            if (Random.value < 0.25f)
            {
                speed += 2f + wave * 0.2f;
                health -= 10 + wave * 4;
                value += 2 + wave * 2;
            }
        }

        maxHealth = health;

        float yOffset = isBoss ? 12f : 3f;
        GameObject hb = Instantiate(healthBarPrefab, transform.position + Vector3.up * yOffset, Quaternion.identity, transform);
        healthBar = hb.GetComponent<HealthBar>();
        healthBar.SetHealth(1f);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        healthBar.SetHealth((float)health / maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

void Die()
{
    float moneyMultiplier = 1f;

    if (WaveSpawner.CurrentWave >= 12)
    {
        moneyMultiplier = 0.6f;
    }

    PlayerStats.Money += Mathf.RoundToInt(value * moneyMultiplier);
    Destroy(gameObject);
}

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }
}