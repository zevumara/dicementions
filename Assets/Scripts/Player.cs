using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public static event Action onPlayerDamaged;
    public float moveSpeed = 5f, hp, maxHp = 5f;
    public Rigidbody2D rigidBody;
    public Camera mainCamera;
    public Animator animator;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    public GameObject weapon;
    public GameObject enemy1;
    public GameObject enemy2;
    private Vector2 movement;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private CameraShake cameraShake;
    private Transform weaponHolder;
    private GameObject currentWeapon;
    private bool canMove = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // hp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        cameraShake = FindFirstObjectByType<CameraShake>();
        weaponHolder = transform.Find("Weapon Holder");
        if (weapon != null && weaponHolder != null)
        {
            EquipWeapon();
        }
    }
    public void EquipWeapon()
    {
        currentWeapon = Instantiate(weapon, weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
    }
    void Update()
    {
        if (!canMove) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetBool("isMoving", movement.sqrMagnitude > 0.01f);
    }
    void FixedUpdate()
    {
        if (!canMove) return;

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
    }
    IEnumerator FlashDamage()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
    void Die()
    {
        Debug.Log("Game Over");
    }

    public bool isReady()
    {
        if (weapon != null && enemy1 != null && enemy2 != null && hp > 0)
        {
            return true;
        }
        return false;
    }

    public void StartLevelIntro()
    {
        StartCoroutine(LevelIntroSequence());
    }

    private IEnumerator LevelIntroSequence()
    {
        canMove = false;
        float moveTime = 4f;
        float timer = 0f;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;
            rigidBody.linearVelocity = Vector2.up * 1f;
            animator.SetBool("isMoving", true);
            yield return null;
        }

        rigidBody.linearVelocity = Vector2.zero;
        animator.SetBool("isMoving", false);

        // Mostrar cuenta regresiva
        yield return StartCoroutine(LevelManager.Instance.ShowCountdown(3));

        // Activar enemigos

        canMove = true;
    }

    public bool CanShoot()
    {
        if (!canMove) return false;
        return true;
    }
}
