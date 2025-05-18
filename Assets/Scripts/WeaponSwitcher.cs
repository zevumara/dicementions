using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons;

    private int currentIndex = 0;

    void Start()
    {
        ActivateWeapon(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ActivateWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ActivateWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ActivateWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ActivateWeapon(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ActivateWeapon(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) ActivateWeapon(5);
    }

    void ActivateWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        currentIndex = index;
    }
}