using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public float lookAheadDistance = 2f;
    public SpriteRenderer levelBoundsSprite;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float camHalfHeight;
    private float camHalfWidth;
    private Camera mainCamera;
    private Vector3 currentLookAhead;
    private Vector3 lookAheadVelocity;
    public float lookSmoothTime = 0.1f;

    void Start()
    {
        mainCamera = Camera.main;
        camHalfHeight = mainCamera.orthographicSize;
        camHalfWidth = mainCamera.aspect * camHalfHeight;

        Bounds bounds = levelBoundsSprite.bounds;
        minBounds = bounds.min;
        maxBounds = bounds.max;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Dirección desde el jugador hacia el mouse
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = ((Vector2)mouseWorldPos - (Vector2)target.position).normalized;

        // Desplazamiento adicional en la dirección de apuntado
        Vector3 targetLookAhead = (Vector3)(aimDirection * lookAheadDistance);
        currentLookAhead = Vector3.SmoothDamp(currentLookAhead, targetLookAhead, ref lookAheadVelocity, lookSmoothTime);
        Vector3 desiredPosition = target.position + offset + currentLookAhead;

        // Clamp a los límites del mapa
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);
        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed * Time.deltaTime);
    }
}