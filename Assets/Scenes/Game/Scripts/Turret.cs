using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 15f;
    public float fireRate = 1f; 
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public float turnSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private Transform target;

    [Header("Lifetime Settings")]
public float turretLifetime = 15f;
private float lifeTimer = 0f;

public LifetimeBar lifetimeBar;
private LifetimeBar instanceBar;

void Start()
{
    InvokeRepeating("UpdateTarget", 0f, 0.5f);
    
    instanceBar = Instantiate(lifetimeBar, transform);
    instanceBar.transform.localPosition = Vector3.up * 5f;
}


    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        target = (nearestEnemy != null && shortestDistance <= range) ? nearestEnemy.transform : null;
    }

void Update()
{
    if (target != null)
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * turnSpeed
        );

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }
    lifeTimer += Time.deltaTime;

    if (instanceBar != null)
    {
        float remaining = 1f - (lifeTimer / turretLifetime);
        instanceBar.SetLifetime(remaining);
    }

    if (lifeTimer >= turretLifetime)
    {
        if (instanceBar != null) Destroy(instanceBar.gameObject);
        Destroy(gameObject);
    }
}

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(target.position - firePoint.position));
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Seek(target);
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}