using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public static event Action onPlayerDamaged;
    public float moveSpeed = 5f, hp, maxHp = 5f;
    public Rigidbody2D rigidBody;
    public Animator animator;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    public int wins = 0;
    public int defeatedEnemies = 0;
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
    private bool canAim = true;
    private bool isDead = false;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        cameraShake = FindFirstObjectByType<CameraShake>();
        weaponHolder = transform.Find("Weapon Holder");
        if (weapon != null && weaponHolder != null)
        {
            EquipWeapon();
        }
    }
    public void SetNewWeapon(GameObject newWeapon)
    {
        weapon = newWeapon;
    }
    public void EquipWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
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
        if (hp > 0)
        {
            hp -= amount;
            onPlayerDamaged?.Invoke();
            cameraShake.Shake();
            StartCoroutine(FlashDamage());
            FindFirstObjectByType<ScreenFlash>()?.Flash();
        }
        if (hp <= 0)
        {
            Die();
        }
    }
    public void TakePotion(int amount)
    {
        hp += amount;
        if (hp > maxHp)
        {
            maxHp = hp;
        }
        if (hp > 14)
        {
            hp = maxHp = 14;
        }
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
        if (!isDead)
        {
            isDead = true;
            LevelManager.Instance.ShowGameOver();
        }
    }

    public void Reset()
    {
        hp = 0;
        maxHp = 4;
        weapon = null;
        enemy1 = null;
        enemy2 = null;
        wins = 0;
        defeatedEnemies = 0;
        isDead = false;
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
        canAim = true;
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

        canMove = true;
    }

    public bool CanMove()
    {
        if (LevelManager.Instance.IsPaused()) return false;
        return canMove;
    }

    public bool CanShoot()
    {
        if (!CanMove()) return false;
        return true;
    }
    public bool CanAim()
    {
        if (!CanMove()) return false;
        return canAim;
    }

    public void FadeOut(float duration)
    {
        canMove = false;
        canAim = false;
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        float elapsed = 0f;

        Color[] originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].color;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            for (int i = 0; i < renderers.Length; i++)
            {
                Color c = originalColors[i];
                renderers[i].color = new Color(c.r, c.g, c.b, alpha);
            }

            yield return null;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            Color c = originalColors[i];
            renderers[i].color = new Color(c.r, c.g, c.b, 0f);
        }
    }

    public void ResetFade()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in renderers)
        {
            Color c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 1f);
        }
    }
}
