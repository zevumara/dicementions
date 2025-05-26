using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DiceWeapon : DiceBase
{
    protected override string[] Faces => new string[6] {
        "LanzaGranadas", // +X
        "Uzi", // -X
        "Boomerang", // +Y
        "Escopeta", // -Y
        "Bate", // +Z
        "Pistola"  // -Z
    };

    protected override void DiceRolled(int index)
    {
        // Debug.Log("Index: " + index);
        // Debug.Log("Arma: " + Faces[index]);
        string weaponName = Faces[index];
        int weaponIndex = GameManager.Instance.GetWeaponIndexByName("Weapon" + weaponName);

        Sprite sprite = DiceManager.Instance.spritesWeapons[weaponIndex];
        image.sprite = sprite;
        image.enabled = true;

        GameObject weaponPrefab = GameManager.Instance.GetWeaponByName("Weapon" + weaponName);
        Player.Instance.weapon = weaponPrefab;
    }
}