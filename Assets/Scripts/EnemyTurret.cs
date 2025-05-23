using System.Collections;
using UnityEngine;

public class EnemyTurret : EnemyBase, IDamageable
{

    [Header("General")]
    public float hitPoints = 3;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    public GameObject destroyedVersionPrefab;

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
    public float bulletForce = 8f;
    
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private float shootTimer = 0f;
    private float shootDurationTimer = 0f;
    private bool isShooting = false;
    private Color originalColor;
    private bool isPreparing = false;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        shootTimer = Random.Range(2, 10);
        shootCooldown = Random.Range(3, 12);
    }

    void FixedUpdate()
    {
        if (LevelManager.Instance.isPaused()) return;

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

    public void TakeDamage(int amount)
    {
        hitPoints -= amount;
        StartCoroutine(FlashDamage());

        if (hitPoints <= 0)
            Die();
    }

    private IEnumerator FlashDamage()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = isShooting ? shootColor : originalColor;
    }

    public override void Die()
    {
        Instantiate(destroyedVersionPrefab, transform.position, transform.rotation);
        base.Die();
    }

    private IEnumerator PrepareShoot()
    {
        if (LevelManager.Instance.isPaused()) yield break;

        if (isPreparing) yield break;

        isPreparing = true;
        spriteRenderer.color = shootColor;

        yield return new WaitForSeconds(prepareShootDuration);

        // Inicia el disparo
        isShooting = true;
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
}