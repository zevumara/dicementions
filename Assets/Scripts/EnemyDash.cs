using UnityEngine;

public class EnemyDash : EnemyBase, IDamageable
{

    [Header("General")]
    public float hitPoints = 3;
    public float normalSpeed = 1.5f;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 3f;
    public Color dashColor = Color.cyan;
    public float prepareDashDuration = 0.5f;
    private Transform player;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private float currentSpeed;
    private float dashTimer = 0f;
    private float dashDurationTimer = 0f;
    private bool isDashing = false;
    private Color originalColor;
    private Vector2 dashDirection = Vector2.zero;
    private int bulletLayer;
    private bool isPreparing = false;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bulletLayer = LayerMask.NameToLayer("Bullet");
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        currentSpeed = normalSpeed;
        dashTimer = Random.Range(2, 8);
    }

    void FixedUpdate()
    {
        if (LevelManager.Instance.IsPaused()) return;

        Vector2 direction = isDashing ? dashDirection : ((Vector2) player.position - rigidBody.position).normalized;

        // Movimiento
        rigidBody.MovePosition(rigidBody.position + direction * currentSpeed * Time.fixedDeltaTime);

        // Dash cooldown
        dashTimer -= Time.fixedDeltaTime;

        if (!isDashing && dashTimer <= 0f)
        {
            StartCoroutine(PrepareDash());
        }

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
    }

    public void TakeDamage(int amount)
    {
        if (isDashing) return;

        hitPoints -= amount;
        StartCoroutine(FlashDamage());

        if (hitPoints <= 0)
            Die();
    }

    private System.Collections.IEnumerator FlashDamage()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = isDashing ? dashColor : originalColor;
    }

    private System.Collections.IEnumerator PrepareDash()
    {
        if (LevelManager.Instance.IsPaused()) yield break;

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