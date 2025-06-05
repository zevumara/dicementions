using UnityEngine;

public class LookAtCamera: MonoBehaviour
{
    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        if (rect == null)
        {
            Debug.LogError("LookAtCamera: No RectTransform found on this GameObject. Disabling script.");
            enabled = false;
        }
    }

    void Update()
    {
        if (rect != null)
        {
            rect.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);
        }
    }
}