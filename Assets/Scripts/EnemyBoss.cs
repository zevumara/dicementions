using System.Collections;
using UnityEngine;

public class EnemyBoss : MonoBehaviour, IDamageable
{

    [Header("General")]
    public float hitPoints = 100;
    public float normalSpeed = 1.5f;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 3f;
    public Color dashColor = Color.cyan;
    public float prepareDashDuration = 0.5f;

    [Header("Shoot Settings")]
    public float rotationSpeed = 200f;
    public float shootSpeed = 0.2f;
    public float shootDuration = 10f;
    public float shootCooldown = 3f;
    public Color shootColor = Color.cyan;
    public float prepareShootDuration = 0.5f;

    [Header("Projectile Settings")]
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;

    private Transform player;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private CameraShake cameraShake;
    private float currentSpeed;
    private float dashTimer = 0f;
    private float dashDurationTimer = 0f;
    private bool isDashing = false;
    private Color originalColor;
    private Vector2 dashDirection = Vector2.zero;
    private int bulletLayer;
    private bool isPreparing = false;
    private bool isShooting = false;
    private float shootTimer = 0f;
    private float shootDurationTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bulletLayer = LayerMask.NameToLayer("Bullet");
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraShake = FindFirstObjectByType<CameraShake>();
        originalColor = spriteRenderer.color;
        currentSpeed = normalSpeed;
        StartCoroutine(EnemyBehaviorLoop());
    }

    private IEnumerator EnemyBehaviorLoop()
    {
        while (true)
        {
            // Esperar a que no esté ocupado
            while (isDashing || isShooting || isPreparing)
                yield return null;

            // Esperar cooldowns si deseas manejarlos aquí (opcional)
            // yield return new WaitForSeconds(1f);

            // Elegir acción aleatoria
            float choice = Random.value;
            if (choice < 0.5f)
            {
                // Solo iniciar dash si el cooldown ya pasó
                if (!isDashing && dashTimer <= 0f)
                {
                    StartCoroutine(PrepareDash());
                }
            }
            else
            {
                if (!isShooting && shootTimer <= 0f)
                {
                    StartCoroutine(PrepareShoot());
                }
            }

            yield return new WaitForSeconds(1f); // Pequeña pausa entre elecciones
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = isDashing ? dashDirection : ((Vector2)player.position - rigidBody.position).normalized;

        // Movimiento
        rigidBody.MovePosition(rigidBody.position + direction * currentSpeed * Time.fixedDeltaTime);

        // Dash cooldown
        dashTimer -= Time.fixedDeltaTime;

        // Dash en curso
        if (isDashing)
        {
            dashDurationTimer -= Time.fixedDeltaTime;
            if (dashDurationTimer <= 0f)
            {
                // Fin del dash
                isDashing = false;
                currentSpeed = normalSpeed;
                dashTimer = dashCooldown;
                spriteRenderer.color = originalColor;
                // Rehabilita colisiones con balas
                Physics2D.IgnoreLayerCollision(gameObject.layer, bulletLayer, false);
            }
        }

        // Shoot cooldown
        shootTimer -= Time.fixedDeltaTime;

        // Disparando
        if (isShooting)
        {
            shootDurationTimer -= Time.fixedDeltaTime;
            firePoint.Rotate(0f, 0f, rotationSpeed * Time.fixedDeltaTime);
            if (shootDurationTimer <= 0f)
            {
                // Terminó de disparar
                isShooting = false;
                shootTimer = shootCooldown;
                spriteRenderer.color = originalColor;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDashing) return;

        hitPoints -= amount;
        cameraShake.Shake();
        StartCoroutine(FlashDamage());

        if (hitPoints <= 0)
            Die();
    }

    private IEnumerator FlashDamage()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = isDashing ? dashColor : originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator PrepareShoot()
    {
        if (isPreparing) yield break;

        isPreparing = true;
        spriteRenderer.color = shootColor;

        yield return new WaitForSeconds(prepareShootDuration);

        // Inicia el disparo
        isShooting = true;
        currentSpeed = 0f;
        shootDurationTimer = shootDuration;
        spriteRenderer.color = shootColor;
        isPreparing = false;
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (isShooting)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            bullet.layer = LayerMask.NameToLayer("EnemyBullet");
            Rigidbody2D rigidBody = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = firePoint.up;
            rigidBody.AddForce(direction * bulletForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(shootSpeed);
        }
    }

    private IEnumerator PrepareDash()
    {
        if (isPreparing) yield break;

        isPreparing = true;
        spriteRenderer.color = dashColor;

        yield return new WaitForSeconds(prepareDashDuration);

        // Inicia dash
        isDashing = true;
        currentSpeed = dashSpeed;
        dashDurationTimer = dashDuration;
        dashDirection = ((Vector2)player.position - rigidBody.position).normalized;
        spriteRenderer.color = dashColor;
        Physics2D.IgnoreLayerCollision(gameObject.layer, bulletLayer, true);
        isPreparing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }
    }
}