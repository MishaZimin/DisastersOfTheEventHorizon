using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform bar;

    public void SetHealth(float healthNormalized)
    {
        healthNormalized = Mathf.Clamp01(healthNormalized);
        bar.localScale = new Vector3(healthNormalized, 1f, 1f);
    }

    void LateUpdate()
{
    transform.rotation = Quaternion.identity;
}
}