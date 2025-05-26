using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DiceEnemy2 : DiceBase
{
    protected override string[] Faces => new string[6] {
        "Jefe", // +X
        "Jefe", // -X
        "Torreta", // +Y
        "Embiste", // -Y
        "Persigue", // +Z
        "Dispara"  // -Z
    };

    protected override void DiceRolled(int index)
    {
        GameObject enemyPrefab = GameManager.Instance.GetRndEnemyByType(Faces[index]);
        Sprite sprite = enemyPrefab.GetComponent<SpriteRenderer>().sprite;
        image.sprite = sprite;
        image.enabled = true;
        Player.Instance.enemy2 = enemyPrefab;
    }
}