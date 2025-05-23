using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Transform playerSpawnPoint;
    public TMP_Text countdownText;
    public int minEnemies = 20;
    public int maxEnemies = 30;
    public int enemyMultiplier = 5;
    private GameObject exitDoor;
    private bool pause = true;
    private List<Collider2D> spawnZones = new List<Collider2D>();
    private List<EnemyBase> enemies = new List<EnemyBase>();
    private bool clear = false;

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
        exitDoor = GameObject.Find("Exit Door");
        var player = Player.Instance;

        if (player != null)
        {
            player.gameObject.SetActive(true);
            player.ResetFade();
            player.transform.position = playerSpawnPoint.position;
            CameraFollow.Instance.target = player.transform;
            if (player.weapon == null)
            {
                // Si por algún motivo no tiene arma (bug), arranca con la pistola
                player.weapon = GameManager.Instance.GetWeaponByIndex(1);
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
            if (player.enemy1 == null || player.enemy2 == null)
            {
                player.enemy1 = GameManager.Instance.GetEnemyByIndex(3);
                player.enemy2 = GameManager.Instance.GetEnemyByIndex(2);
            }
        }

        // Spawneo de enemigos: buscar todas las zonas con la etiqueta "SpawnZone"
        foreach (var zone in GameObject.FindGameObjectsWithTag("SpawnZone"))
        {
            Collider2D collider = zone.GetComponent<Collider2D>();
            if (collider != null)
                spawnZones.Add(collider);
        }

        int quantity = Random.Range(minEnemies, maxEnemies + 1) + (Player.Instance.wins * enemyMultiplier);

        if (Player.Instance.enemy1.name == "EnemyBoss")
        {
            SpawnEnemiesWithBoss(quantity, Player.Instance.enemy1, Player.Instance.enemy2);
        }
        else if (Player.Instance.enemy2.name == "EnemyBoss")
        {
            SpawnEnemiesWithBoss(quantity, Player.Instance.enemy2, Player.Instance.enemy1);
        }
        else
        {
            SpawnEnemies(quantity);
        }
        
        clear = false;
    }

    void SpawnEnemies(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject prefab = Random.value < 0.5f ? Player.Instance.enemy1 : Player.Instance.enemy2;
            Collider2D zone = spawnZones[Random.Range(0, spawnZones.Count)];
            Vector2 spawnPosition = GetRandomPointInBounds(zone.bounds);
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnEnemiesWithBoss(int quantity, GameObject prefabBoss, GameObject prefabEnemy)
    {
        bool bossSpawned = false;

        for (int i = 0; i < quantity; i++)
        {
            GameObject prefabToSpawn;

            if (!bossSpawned && Random.value < 0.1f)
            {
                prefabToSpawn = prefabBoss;
                bossSpawned = true;
            }
            else
            {
                prefabToSpawn = prefabEnemy;
            }

            Collider2D zone = spawnZones[Random.Range(0, spawnZones.Count)];
            Vector2 spawnPosition = GetRandomPointInBounds(zone.bounds);

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }

        // Si nunca se spawneó el boss, forzarlo en el último
        if (!bossSpawned)
        {
            Collider2D zone = spawnZones[Random.Range(0, spawnZones.Count)];
            Vector2 spawnPosition = GetRandomPointInBounds(zone.bounds);
            Instantiate(prefabBoss, spawnPosition, Quaternion.identity);
        }
    }

    Vector2 GetRandomPointInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    public IEnumerator ShowCountdown(int seconds)
    {
        countdownText.gameObject.SetActive(true);

        for (int i = seconds; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        string[] startingText = { "¡Que comience el juego!", "¡Buena suerte!", "¡A jugar!", "¡La suerte está echada!" };
        countdownText.text = startingText[Random.Range(0, startingText.Length)];
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        pause = false;
    }

    public bool isPaused()
    {
        return pause;
    }

    public void RegisterEnemy(EnemyBase enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyBase enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            AllEnemiesDefeated();
        }
    }

    private void AllEnemiesDefeated()
    {
        clear = true;
        Player.Instance.wins++;

        Collider2D doorCollider = exitDoor.GetComponent<Collider2D>();
        doorCollider.isTrigger = true;

        Transform opened = exitDoor.transform.Find("Opened");
        opened.gameObject.SetActive(true);
    }

    public bool isLevelClear()
    {
        return clear;
    }
}
