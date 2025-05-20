using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    public RawImage rawImage;
    public Vector2 scrollSpeed = new Vector2(0.1f, -0.1f); // Diagonal hacia abajo

    private Vector2 uvOffset = Vector2.zero;

    void Update()
    {
        uvOffset += scrollSpeed * Time.deltaTime;

        // Calcular cuántas veces repetir la textura en base al tamaño visual del RawImage
        Vector2 imageSize = rawImage.rectTransform.rect.size;
        Vector2 textureSize = new Vector2(rawImage.texture.width, rawImage.texture.height);
        Vector2 tiling = imageSize / textureSize;

        rawImage.uvRect = new Rect(uvOffset, tiling);
    }
}