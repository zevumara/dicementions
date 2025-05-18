using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public GameObject hitEffect;
    public float speed = 100;
    public float rotateSpeed = 720f;
    public float timeBeforeReturn = 0.6f;
    private Vector2 travelDirection;
    private Transform player;
    private bool returning = false;
    private Rigidbody2D rigidBody;
    private float timer = 0f;
    private WeaponBoomerang weapon;
    private CameraShake cameraShake;

    public void Initialize(Transform playerTransform, Vector2 direction, WeaponBoomerang weaponSource)
    {
        player = playerTransform;
        travelDirection = direction;
        weapon = weaponSource;
    }

    void Start()
    {
        cameraShake = FindFirstObjectByType<CameraShake>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        timer += Time.deltaTime;

        if (!returning)
        {
            rigidBody.MovePosition(rigidBody.position + travelDirection * speed * Time.fixedDeltaTime);

            if (timer >= timeBeforeReturn)
            {
                returning = true;
            }
        }
        else
        {
            Vector2 directionToPlayer = ((Vector2)player.position - rigidBody.position).normalized;
            rigidBody.MovePosition(rigidBody.position + directionToPlayer * speed * Time.fixedDeltaTime);

            if (Vector2.Distance(rigidBody.position, player.position) < 2f)
            {
                // Avisar al arma que el boomerang volvió y destruir
                weapon?.OnBoomerangReturn();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.3f);
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                cameraShake.Shake();
                damageable.TakeDamage(1);
            }
        }
    }
}