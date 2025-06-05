using UnityEngine;
using FMODUnity;

public class PlayerBat : MonoBehaviour
{
    public string fmodEventPath = "event:/SFX/Bat"; // Asignación directa o desde el Inspector

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
