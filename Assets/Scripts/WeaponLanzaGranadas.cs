using UnityEngine;

public class WeaponLanzaGranadas : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    private float holdTime = 0f;
    private bool isCharging = false;
    private Transform firePoint;

    void Start()
    {
        firePoint = transform.Find("Firepoint");
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Input.GetButtonDown("Fire1"))
        {
            isCharging = true;
            holdTime = 0f;
        }

        if (Input.GetButton("Fire1") && isCharging)
        {
            holdTime += Time.deltaTime * 1.1f;
        }

        if (Input.GetButtonUp("Fire1") && isCharging)
        {
            isCharging = false;
            Shoot(holdTime);
        }
    }

    void Shoot(float chargeDuration)
    {
        if (Player.Instance.CanShoot())
        {
            Vector2 shootDirection = firePoint.up;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 90));
            bullet.GetComponent<Granade>().Initialize(shootDirection, chargeDuration);
        }
    }
}
