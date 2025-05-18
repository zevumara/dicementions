using System.Collections;
using UnityEngine;

public class EnemyShoot : MonoBehaviour, IDamageable
{

    [Header("General")]
    public float hitPoints = 3;
    public float normalSpeed = 1.5f;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Shoot Settings")]
    public float shootDuration = 0.5f;
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
    private float currentSpeed;
    private float shootTimer = 0f;
    private float shootDurationTimer = 0f;
    private bool isShooting = false;
    private Color originalColor;
    private bool isPreparing = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        currentSpeed = normalSpeed;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2) player.position - rigidBody.position).normalized;

        // Movimiento
        rigidBody.MovePosition(rigidBody.position + direction * currentSpeed * Time.fixedDeltaTime);

        // Shoot cooldown
        shootTimer -= Time.fixedDeltaTime;

        if (!isShooting && shootTimer <= 0f)
        {
            StartCoroutine(PrepareShoot());
        }

        // Disparando
        if (isShooting)
        {
            shootDurationTimer -= Time.fixedDeltaTime;
            if (shootDurationTimer <= 0f)
            {
                // Terminó de disparar
                isShooting = false;
                currentSpeed = normalSpeed;
                shootTimer = shootCooldown;
                spriteRenderer.color = originalColor;
            }
        }
    }

    private IEnumerator ShootRoutine()
    {
        while (isShooting)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            bullet.layer = LayerMask.NameToLayer("EnemyBullet");
            Rigidbody2D rigidBody = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = ((Vector2) player.position - rigidBody.position).normalized;
            rigidBody.AddForce(direction * bulletForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator FlashDamage()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = isShooting ? shootColor : originalColor;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        hitPoints -= amount;
        StartCoroutine(FlashDamage());

        if (hitPoints <= 0)
            Die();
    }
}