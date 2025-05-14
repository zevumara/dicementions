using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLight : MonoBehaviour
{
    private Light2D light2D;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 0.1f;

    private float timer;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            float intensity = Random.Range(minIntensity, maxIntensity);
            light2D.intensity = intensity;
            timer = flickerSpeed;
        }
    }
}