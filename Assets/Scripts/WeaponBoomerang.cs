using UnityEngine;

public class WeaponBoomerang : MonoBehaviour
{
    public Camera mainCamera;
    public Transform player;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    private bool boomerangInFlight = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Debug.Log(boomerangInFlight);
        if (boomerangInFlight) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - firePoint.position).normalized;

        GameObject boomerang = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        boomerang.GetComponent<Boomerang>().Initialize(player, direction, this);

        boomerangInFlight = true;
        spriteRenderer.enabled = false;
    }

    public void OnBoomerangReturn()
    {
        boomerangInFlight = false;
        spriteRenderer.enabled = true;
    }
}
