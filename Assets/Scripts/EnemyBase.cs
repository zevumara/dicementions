using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected virtual void Start()
    {
        LevelManager.Instance.RegisterEnemy(this);
    }

    public virtual void Die()
    {
        LevelManager.Instance.UnregisterEnemy(this);
        Destroy(gameObject);
    }
}