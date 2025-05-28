using UnityEngine;

public class CreditsDice: MonoBehaviour
{
    public Transform initialposition;
    private Vector3 savePosition;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        savePosition = initialposition.position;
        gameObject.transform.position = savePosition;
        float randomAngle = Random.Range(0f, 360f);
        initialposition.rotation = Quaternion.Euler(randomAngle, randomAngle, randomAngle);
    }
    private void OnDisable()
    {
        gameObject.transform.position = savePosition;
        rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero;
    }
}
