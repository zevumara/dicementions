using UnityEngine;

public class WeaponEscopeta : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletForce = 30f;
    public int pellets = 6;
    public float bulletLifetime = 0.3f;
    private float recoilForce = 200f;
    private float offsetAmount = 0.7f;
    private Camera levelCamera;
    private Transform player;
    private Transform firePoint;

    void Start()
    {
        player = Player.Instance.transform;
        levelCamera = LevelManager.Instance.mainCamera;
        firePoint = transform.Find("Firepoint");
    }

    void Update()
    {
        // Rotar el arma hacia el mouse
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
        float centerIndex = (pellets - 1) / 2f;

        for (int i = 0; i < pellets; i++)
        {
            float lateralOffset = (i - centerIndex) * offsetAmount;
            Vector3 spawnPosition = firePoint.position + firePoint.right * lateralOffset;
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, 90);
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.lifetime = 0.3f;
            Rigidbody2D rigidBody = bullet.GetComponent<Rigidbody2D>();
            rigidBody.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            Destroy(bullet, bulletLifetime);
        }

        player.position += (Vector3)(-firePoint.up * recoilForce * 0.01f);
    }
}