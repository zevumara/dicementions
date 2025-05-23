using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Sprite fullHeart, halfHeart, emptyHeart;
    private Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch(status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }
}
public enum HeartStatus
{
    Empty = 0,
    Half = 1,
    Full = 2
}