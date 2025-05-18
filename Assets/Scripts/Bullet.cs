using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public float lifetime = 3f;
    private SpriteRenderer sprite;
    private float elapsed;
    private CameraShake cameraShake;

    void Start()
    {
        cameraShake = FindFirstObjectByType<CameraShake>();
        sprite = GetComponent<SpriteRenderer>();
        elapsed = 0f;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / lifetime);
        Color c = sprite.color;
        c.a = Mathf.Lerp(1f, 0f, t);
        sprite.color = c;

        if (elapsed >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.3f);
            Destroy(gameObject);
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                cameraShake.Shake();
                damageable.TakeDamage(1);
            }
        }
    }
}