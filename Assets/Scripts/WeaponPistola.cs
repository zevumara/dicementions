using Unity.VisualScripting;
using UnityEngine;

public class WeaponPistola : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    private Transform firePoint;

    void Start()
    {
        firePoint = transform.Find("Firepoint");
    }

    void Update()
    {
        if (!Player.Instance.CanAim()) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
