using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ExitLight : MonoBehaviour
{
    private Light2D light2D;
    public float minIntensity = 2f;
    public float maxIntensity = 6f;
    public float pulseSpeed = 1f;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        if (!LevelManager.Instance.isLevelClear()) return;
        float t = (Mathf.Sin(Time.time * pulseSpeed * Mathf.PI * 2f) + 1f) / 2f;
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}