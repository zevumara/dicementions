using UnityEngine;

public class WeaponBoomerang : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    private bool boomerangInFlight = false;
    private SpriteRenderer spriteRenderer;
    private Camera levelCamera;
    private Transform player;
    private Transform firePoint;

    void Start()
    {
        player = Player.Instance.transform;
        levelCamera = LevelManager.Instance.mainCamera;
        spriteRenderer = GetComponent<SpriteRenderer>();
        firePoint = transform.Find("Firepoint");
    }
    void Update()
    {
        Vector3 mousePosition = levelCamera.ScreenToWorldPoint(Input.mousePosition);
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
        if (Player.Instance.CanShoot())
        {
            if (boomerangInFlight) return;

            Vector3 mousePos = levelCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - firePoint.position).normalized;

            GameObject boomerang = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            boomerang.GetComponent<Boomerang>().Initialize(player, direction, this);

            boomerangInFlight = true;
            spriteRenderer.enabled = false;
        }
    }

    public void OnBoomerangReturn()
    {
        boomerangInFlight = false;
        spriteRenderer.enabled = true;
    }
}
