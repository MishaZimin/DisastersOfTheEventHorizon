using UnityEngine;

public class LifetimeBar : MonoBehaviour
{
    public Transform bar; // Ссылка на Transform полосы (например, дочерний объект)

    public void SetLifetime(float normalizedTime)
    {
        normalizedTime = Mathf.Clamp01(normalizedTime);
        bar.localScale = new Vector3(normalizedTime, 1f, 1f); // Меняем scale по X
    }

    void LateUpdate()
    {
        // Всегда смотрим на камеру (Billboard-эффект)
        transform.rotation = Quaternion.identity;
    }
}