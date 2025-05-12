using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hitPoints = 3;
    public float speed = 1.5f;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    private Transform player;
    private Rigidbody2D rigidBody;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        // Movimiento hacia el jugador
        Vector2 direction = ((Vector2)player.position - rigidBody.position).normalized;
        rigidBody.MovePosition(rigidBody.position + direction * speed * Time.fixedDeltaTime);

        // Rotación hacia el jugador
        Vector2 lookDir = (Vector2)player.position - rigidBody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rigidBody.rotation = angle;
    }

    public void TakeDamage(int amount)
    {
        hitPoints -= amount;
        StartCoroutine(FlashDamage());

        if (hitPoints <= 0)
        {
            Die();
        }
    }
    System.Collections.IEnumerator FlashDamage()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        // Acá podés poner una animación o efecto antes de destruir
        Destroy(gameObject);
    }
}