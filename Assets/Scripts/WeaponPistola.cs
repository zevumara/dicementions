using UnityEngine;

public class WeaponPistola : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    private Camera levelCamera;
    private Transform firePoint;

    void Start()
    {
        levelCamera = LevelManager.Instance.mainCamera;
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
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 90));
            Rigidbody2D rigidBody = bullet.GetComponent<Rigidbody2D>();
            rigidBody.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        }
    }
}
