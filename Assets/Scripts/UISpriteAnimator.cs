
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimator : MonoBehaviour
{
    public Image image;
    public Sprite[] frames;
    public float frameRate = 10f;

    private int currentFrame;
    private float timer;

    void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % frames.Length;
            image.sprite = frames[currentFrame];
        }
    }
}