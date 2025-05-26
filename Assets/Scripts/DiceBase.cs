using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public abstract class DiceBase : MonoBehaviour
{
    protected abstract string[] Faces { get; }
    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;
    public float rotationSpeed = 30f;
    public Vector3 floatOffset = new Vector3(0, 0.25f, 0);
    public float floatSpeed = 2f;
    public Image image;
    private Renderer rend;
    private Rigidbody rigidBody;
    private bool physicsEnabled = false;
    private Vector3 originalPosition;
    private bool waitingToStop = false;
    private float stillTime = 0f;
    private float stopThreshold = 0.2f;
    private float throwForce = 0.8f;
    private float throwTorque = 2.1f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = normalColor;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        originalPosition = transform.position;
    }

    protected virtual void DiceRolled(int index) {
        Debug.Log("La cara visible es: " + index);
    }

    void FixedUpdate()
    {
        if (physicsEnabled)
        {
            if (waitingToStop)
            {
                if (rigidBody.IsSleeping() ||
                    (rigidBody.linearVelocity.magnitude < 0.05f && rigidBody.angularVelocity.magnitude < 0.05f))
                {
                    stillTime += Time.fixedDeltaTime;
                    if (stillTime >= stopThreshold)
                    {
                        waitingToStop = false;
                        GetDiceFace();
                    }
                }
                else
                {
                    stillTime = 0f;
                }
            }
        }
        else
        {
            // Flotar y rotar
            Vector3 axis = new Vector3(1f, 1f, 1f).normalized;
            transform.Rotate(axis * rotationSpeed * Time.deltaTime, Space.Self);
            float y = Mathf.Sin(Time.time * floatSpeed) * floatOffset.y;
            transform.position = originalPosition + new Vector3(0, y, 0);
        }
    }

    public void OnHoverEnter()
    {
        if (physicsEnabled) return;
        rend.material.color = hoverColor;
    }

    public void OnHoverExit()
    {
        if (physicsEnabled) return;
        rend.material.color = normalColor;
    }

    public void OnClick()
    {
        if (physicsEnabled) return;
        rend.material.color = normalColor;
        physicsEnabled = true;
        rigidBody.isKinematic = false;
        // Lanzamiento hacia arriba + dirección aleatoria
        float rndForce = Random.Range(-throwForce, throwForce);
        rigidBody.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
        // Agitación rotacional agresiva
        float rndTorque = Random.Range(-throwForce, throwForce);
        Vector3 torqueDirection = new Vector3(1, 1, 0).normalized;
        rigidBody.AddTorque(torqueDirection * throwTorque, ForceMode.Impulse);

        waitingToStop = true;
        stillTime = 0f;
        Player.Instance.TakePotion(2);
    }
    void GetDiceFace()
    {
        // Dirección desde el cubo hacia la cámara
        Vector3 toCam = (Camera.main.transform.position - transform.position).normalized;

        // Convertir esa dirección al espacio local del cubo
        Vector3 localDir = transform.InverseTransformDirection(toCam);

        // Analizar qué componente es dominante
        Vector3 absDir = new Vector3(Mathf.Abs(localDir.x), Mathf.Abs(localDir.y), Mathf.Abs(localDir.z));

        int faceIndex = -1;

        if (absDir.x > absDir.y && absDir.x > absDir.z)
        {
            faceIndex = localDir.x > 0 ? 0 : 1; // +X o -X
        }
        else if (absDir.y > absDir.z)
        {
            faceIndex = localDir.y > 0 ? 2 : 3; // +Y o -Y
        }
        else
        {
            faceIndex = localDir.z > 0 ? 4 : 5; // +Z o -Z
        }

        DiceRolled(faceIndex);
    }
}