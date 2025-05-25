using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected virtual void Start()
    {
        LevelManager.Instance.RegisterEnemy(this);
    }

    public virtual void Die()
    {
        Player.Instance.defeatedEnemies++;
        LevelManager.Instance.UnregisterEnemy(this);
        Destroy(gameObject);
    }
}