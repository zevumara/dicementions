using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;
    private Vector3 initialPosition;
    private float shakeTimeRemaining = 0f;
    private float currentMagnitude = 0.1f;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            Vector2 offset = Random.insideUnitCircle * currentMagnitude;
            transform.localPosition = initialPosition + new Vector3(offset.x, offset.y, 0f);
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = initialPosition;
        }
    }

    public void Shake(float duration = 0.1f, float magnitude = 0.1f)
    {
        shakeTimeRemaining = duration;
        currentMagnitude = magnitude;
    }
}