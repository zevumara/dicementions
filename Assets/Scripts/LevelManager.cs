using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Transform playerSpawnPoint;
    public Camera mainCamera;
    public TMP_Text countdownText;

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
        var player = Player.Instance;

        if (player != null)
        {
            player.gameObject.SetActive(true);
            player.transform.position = playerSpawnPoint.position;
            CameraFollow.Instance.target = player.transform;
            if (player.weapon == null)
            {
                // Si por algún motivo no tiene arma (bug), arranca con la pistola
                player.weapon = GameManager.Instance.GetWeaponByIndex(0);
            }
            player.EquipWeapon();
            player.StartLevelIntro();
        }

        // Solo para testing, borrar
        if (Application.isEditor)
        {
            if (player.hp == 0)
            {
                player.maxHp = 6;
                player.hp = 6;
            }
        }
    }

    public IEnumerator ShowCountdown(int seconds)
    {
        countdownText.gameObject.SetActive(true);

        for (int i = seconds; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "¡Que comience el juego!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
    }
}
