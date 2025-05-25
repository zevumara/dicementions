using UnityEngine;

public class WeaponEscopeta : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletForce = 30f;
    public int pellets = 6;
    public float bulletLifetime = 0.3f;
    private float recoilForce = 100f;
    private float offsetAmount = 0.7f;
    private Transform player;
    private Transform firePoint;
    public float fireRate = 0.8f;
    private float nextFireTime = 0f;

    void Start()
    {
        player = Player.Instance.transform;
        firePoint = transform.Find("Firepoint");
    }

    void Update()
    {
        if (!Player.Instance.CanAim()) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (Player.Instance.CanShoot())
        {
            float centerIndex = (pellets - 1) / 2f;

            for (int i = 0; i < pellets; i++)
            {
                float lateralOffset = (i - centerIndex) * offsetAmount;
                Vector3 spawnPosition = firePoint.position + firePoint.right * lateralOffset;
                Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, 90);
                GameObject bullet = Instantiate(bulletPrefab, spawnPosition, rotation);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.lifetime = bulletLifetime;
                Rigidbody2D rigidBody = bullet.GetComponent<Rigidbody2D>();
                rigidBody.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet, bulletLifetime);
            }

            player.position += (Vector3)(-firePoint.up * recoilForce * 0.01f);
        }
    }
}