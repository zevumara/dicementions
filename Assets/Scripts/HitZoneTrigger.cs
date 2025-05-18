using UnityEngine;

public class HitZoneTrigger : MonoBehaviour
{
    public GameObject hitEffect;
    private CameraShake cameraShake;

    void Start()
    {
        cameraShake = FindFirstObjectByType<CameraShake>();
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameObject effect = Instantiate(hitEffect, collision.transform.position, Quaternion.identity);
            Destroy(effect, 0.3f);
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                cameraShake.Shake();
                damageable.TakeDamage(1);
            }
        }
    }
}