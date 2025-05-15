using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;
    private Vector3 initialPosition;
    private float shakeTimeRemaining = 0f;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            Vector2 offset = Random.insideUnitCircle * shakeMagnitude;
            transform.localPosition = initialPosition + new Vector3(offset.x, offset.y, 0f);
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = initialPosition;
        }
    }

    public void Shake()
    {
        shakeTimeRemaining = shakeDuration;
    }
}