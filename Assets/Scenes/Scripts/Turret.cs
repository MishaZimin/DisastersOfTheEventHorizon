using UnityEngine;

public class Turret : MonoBehaviour
{
    public float range = 15f;         // Дальность обнаружения врагов
    public string enemyTag = "Enemy"; // Тег врагов
    public float turnSpeed = 5f;      // Скорость поворота

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
            return; // Если нет цели — не поворачиваемся

        // 🔄 Поворот цилиндра в сторону врага
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * turnSpeed
        );
    }

    // Визуализация радиуса атаки в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}