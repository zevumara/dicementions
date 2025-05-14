using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    public Image flashImage;
    public Color flashColor = new Color(1f, 0f, 0f, 0.4f);
    public float flashDuration = 0.2f;

    public void Flash()
    {
        StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        flashImage.color = flashColor;

        float t = 0f;
        Color startColor = flashColor;
        Color endColor = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);

        while (t < flashDuration)
        {
            t += Time.deltaTime;
            flashImage.color = Color.Lerp(startColor, endColor, t / flashDuration);
            yield return null;
        }

        flashImage.color = endColor;
    }
}