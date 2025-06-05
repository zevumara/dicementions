using UnityEngine;
using FMODUnity;

public class PlayerBoomerang : MonoBehaviour
{
    public string fmodEventPath = "event:/SFX/Boomerang"; 

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
