using UnityEngine;
using FMODUnity;

public class PlayerCollisionSFX : MonoBehaviour
{
    public string screamEventPath = "event:/SFX/Screams";

    // Lista de nombres válidos de proyectiles enemigos
    private string[] enemyBulletTags = new string[]
    {
        "EnemyBulletAgua",
        "EnemyBulletBall",
        "EnemyBulletBoleadoras",
        "EnemyBulletCard",
        "EnemyBulletFuego",
        "EnemyBulletVerde"
    };

    void OnCollisionEnter(Collision collision)
    {
        string otherName = collision.gameObject.name;

        foreach (string tag in enemyBulletTags)
        {
            if (otherName.Contains(tag))
            {
                if (!string.IsNullOrEmpty(screamEventPath))
                {
                    RuntimeManager.PlayOneShot(screamEventPath);
                }
                else
                {
                    Debug.LogWarning("Ruta del evento FMOD no asignada.");
                }

                break; // Evita reproducir múltiples veces si hay coincidencia múltiple
            }
        }
    }
}
