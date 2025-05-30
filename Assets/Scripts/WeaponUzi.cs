using UnityEngine;
using System.Collections;

public class WeaponUzi : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletForce = 20f;

    [Header("Burst Settings")]
    public int bulletsPerBurst = 5;
    public float timeBetweenBullets = 0.1f;
    public float timeBetweenBursts = 0.5f;
    private Coroutine firingCoroutine;
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

        // Inicia o detiene el disparo continuo
        if (Input.GetButtonDown("Fire1"))
        {
            if (firingCoroutine == null)
                firingCoroutine = StartCoroutine(BurstFire());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
                firingCoroutine = null;
            }
        }
    }

    IEnumerator BurstFire()
    {
        while (true)
        {
            for (int i = 0; i < bulletsPerBurst; i++)
            {
                Shoot();
                yield return new WaitForSeconds(timeBetweenBullets);
            }
            yield return new WaitForSeconds(timeBetweenBursts);
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