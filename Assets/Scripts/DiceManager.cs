using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance { get; private set; }
    public Sprite[] spritesWeapons;
    public Sprite[] spritesEnemies;
    public Image imageWeapon;
    public Image imageEnemy1;
    public Image imageEnemy2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (Player.Instance.weapon != null)
        {
            string weaponName = Player.Instance.weapon.name;
            imageWeapon.sprite = spritesWeapons[GameManager.Instance.GetWeaponIndexByName(weaponName)];
            imageWeapon.enabled = true;
        }
        if (Player.Instance.enemy1 != null)
        {
            imageEnemy1.sprite = Player.Instance.enemy1.GetComponent<SpriteRenderer>().sprite;
            imageEnemy1.enabled = true;
        }
        if (Player.Instance.enemy2 != null)
        {
            imageEnemy2.sprite = Player.Instance.enemy2.GetComponent<SpriteRenderer>().sprite;
            imageEnemy2.enabled = true;
        }
    }
}
