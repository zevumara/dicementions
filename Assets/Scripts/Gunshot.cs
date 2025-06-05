using UnityEngine;
using FMODUnity;

public class PlayerShotgun : MonoBehaviour
{
    public string fmodEventPath = "event:/SFX/Gun"; // Asignación directa o desde el Inspector

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!string.IsNullOrEmpty(fmodEventPath))
            {
                RuntimeManager.PlayOneShot(fmodEventPath);
            }
            else
            {
                Debug.LogWarning("Ruta del evento FMOD no asignada.");
            }
        }
    }
}
