using UnityEngine;

public class EnemyChase : EnemyBase, IDamageable
{
    [Header("General")]
    public float hitPoints = 3;
    public float normalSpeed = 1.5f;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private Transform player;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private float currentSpeed;
    private Color originalColor;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSpeed = normalSpeed;
        originalColor = spriteRenderer.color;
    }

    void FixedUpdate()
    {
        if (LevelManager.Instance.isPaused()) return;

        Vector2 direction = ((Vector2) player.position - rigidBody.position).normalized;

        // Movimiento
        rigidBody.MovePosition(rigidBody.position + direction * currentSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int amount)
    {
        hitPoints -= amount;
        StartCoroutine(FlashDamage());

        if (hitPoints <= 0)
            Die();
    }

    private System.Collections.IEnumerator FlashDamage()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }
    }
}