using UnityEngine;

public class WeaponBate : MonoBehaviour
{
    public Transform weapon;
    public Transform hitZone;
    public float swingDuration = 0.2f;
    public float swingAngle = 180f;
    private bool isSwinging = false;
    private float swingTimer = 0f;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private Camera levelCamera;

    void Start()
    {
        levelCamera = LevelManager.Instance.mainCamera;
        hitZone.gameObject.SetActive(false);
    }

    void Update()
    {
        Vector3 mousePosition = levelCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

        if (Input.GetButtonDown("Fire1") && !isSwinging)
        {
            Shoot();
        }

        if (isSwinging)
        {
            swingTimer += Time.deltaTime;
            float t = Mathf.Clamp01(swingTimer / swingDuration);
            weapon.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            if (swingTimer >= swingDuration)
                EndSwing();
        }
    }

    void Shoot()
    {
        if (Player.Instance.CanShoot())
        {
            isSwinging = true;
            swingTimer = 0f;

            Vector3 mousePosition = levelCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - transform.position;
            float sign = Mathf.Sign(direction.x);

            initialRotation = Quaternion.Euler(0f, 0f, swingAngle * 0.5f * sign);
            targetRotation = Quaternion.Euler(0f, 0f, -swingAngle * 0.5f * sign);

            weapon.localRotation = initialRotation;
            hitZone.gameObject.SetActive(true);
        }
    }

    void EndSwing()
    {
        isSwinging = false;
        swingTimer = 0f;
        weapon.localRotation = Quaternion.identity;
        hitZone.gameObject.SetActive(false);
    }
}