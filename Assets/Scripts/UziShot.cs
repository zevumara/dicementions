using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerUzi : MonoBehaviour
{
    public string fmodEventPath = "event:/SFX/Uzi";

    private EventInstance eventInstance;
    private bool isPlaying = false;

    void Update()
    {
        // Comienza a reproducir mientras se mantiene presionado el botón izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            if (!string.IsNullOrEmpty(fmodEventPath))
            {
                eventInstance = RuntimeManager.CreateInstance(fmodEventPath);
                eventInstance.start();
                isPlaying = true;
            }
            else
            {
                Debug.LogWarning("Ruta del evento FMOD no asignada.");
            }
        }

        // Detiene la reproducción cuando se suelta el botón
        if (Input.GetMouseButtonUp(0) && isPlaying)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
            isPlaying = false;
        }
    }
}
