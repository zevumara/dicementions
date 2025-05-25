using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public TMP_Text roomsNumberText;
    public TMP_Text enemiesNumberText;

    void Start()
    {
        roomsNumberText.text = "SALAS SUPERADAS: " + Player.Instance.wins;
        enemiesNumberText.text = "ENEMIGOS DERROTADOS: " + Player.Instance.defeatedEnemies;
    }

}
