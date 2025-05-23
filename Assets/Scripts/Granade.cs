using UnityEngine;

public class Granade : MonoBehaviour
{
    public GameObject hitEffect;
    public float lifetime = 3f;
    public float explosionRadius = 4f;
    public int damage = 3;
    public float moveDuration = 1.5f;
    public float moveSpeed = 7f;
    private AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    private Vector2 direction;
    private float moveTimer = 0f;
    private float lifeTimer = 0f;
    private Rigidbody2D rigidBody;
    private CameraShake cameraShake;
    private LayerMask enemyLayer;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        cameraShake = FindFirstObjectByType<CameraShake>();
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    }

    public void Initialize(Vector2 shootDirection, float duration)
    {
        direction = shootDirection;
        moveDuration = Mathf.Clamp(duration, 0.1f, 2f);
    }

    void FixedUpdate()
    {
        if (LevelManager.Instance.isPaused()) return;
        
        transform.Rotate(Vector3.forward * 720f * Time.deltaTime);

        lifeTimer += Time.fixedDeltaTime;

        if (moveTimer < moveDuration)
        {
            moveTimer += Time.fixedDeltaTime;
            float speedMultiplier = speedCurve.Evaluate(moveTimer / moveDuration);
            Vector2 offset = direction.normalized * moveSpeed * speedMultiplier * Time.fixedDeltaTime;
            rigidBody.MovePosition(rigidBody.position + offset);
        }

        if (lifeTimer >= lifetime)
        {
            Explode();
        }
    }

    void Explode()
    {
        cameraShake.Shake(0.5f, 0.5f);

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.3f);

        // Detectar enemigos dentro del radio
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}