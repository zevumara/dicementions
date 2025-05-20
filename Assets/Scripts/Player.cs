using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action onPlayerDamaged;
    public float moveSpeed = 5f, hp, maxHp = 5f;
    public Rigidbody2D rigidBody;
    public Camera mainCamera;
    public Animator animator;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    public int weaponID = 0;
    public int enemy1ID = 0;
    public int enemy2ID = 0;
    public bool isReady = false;
    private Vector2 movement;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private CameraShake cameraShake;
    
    private void Start()
    {
        // hp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        cameraShake = FindFirstObjectByType<CameraShake>();
    }
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetBool("isMoving", movement.sqrMagnitude > 0.01f);
    }
    void FixedUpdate()
    {
        rigidBody.linearVelocity = movement * moveSpeed;
    }
    public void TakeDamage(int amount)
    {
        hp -= amount;
        onPlayerDamaged?.Invoke();
        cameraShake.Shake();
        StartCoroutine(FlashDamage());
        FindFirstObjectByType<ScreenFlash>()?.Flash();

        if (hp <= 0)
        {
            Die();
        }
    }
    public void TakePotion(int amount)
    {
        hp += amount;
        onPlayerDamaged?.Invoke();
        cameraShake.Shake();
        StartCoroutine(FlashDamage());
        FindFirstObjectByType<ScreenFlash>()?.Flash();
    }
    System.Collections.IEnumerator FlashDamage()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
    void Die()
    {
        Debug.Log("Game Over");
    }
}
