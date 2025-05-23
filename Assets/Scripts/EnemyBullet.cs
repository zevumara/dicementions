using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject hitEffect;

    void Start()
    {
        Destroy(gameObject, 6f);
    }

    void Update()
    {
        if (LevelManager.Instance.isPaused()) return;

        transform.Rotate(0f, 0f, 360f * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.3f);
            Destroy(gameObject);
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
        }
    }
}