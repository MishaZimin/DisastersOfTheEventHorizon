using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 15f;         // Дальность обнаружения врагов
    public float fireRate = 1f;       // Скорость стрельбы (выстрелов в секунду)
    private float fireCountdown = 0f; // Таймер до следующего выстрела

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy"; // Тег врагов
    public float turnSpeed = 5f;      // Скорость поворота
    public GameObject bulletPrefab;   // Префаб пули
    public Transform firePoint;      // Точка выстрела

    private Transform target;         // Текущая цель

    void Start()
    {
        // Поиск новой цели каждые 0.5 сек (оптимизация)
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
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

        // Обновляем цель, если враг в радиусе атаки
        target = (nearestEnemy != null && shortestDistance <= range) ? nearestEnemy.transform : null;
    }

    void Update()
    {
        if (target == null)
            return;

        // 🔄 Поворот цилиндра в сторону врага
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * turnSpeed
        );

        // Стрельба
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(target.position - firePoint.position));
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Seek(target);
        
        // Альтернатива без Seek():
        // bulletGO.GetComponent<Rigidbody>().velocity = (target.position - firePoint.position).normalized * bullet.speed;
    }

    // Визуализация радиуса атаки в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}