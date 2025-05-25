using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        if (Player.Instance.isReady())
        {
            activate();
        }
        else
        {
            deactivate();
        }
    }

    public void deactivate()
    {
        button.interactable = false;
        Color c = button.image.color;
        c.a = 0.1f;
        button.image.color = c;
        button.GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1, 0.1f);
    }

    public void activate()
    {
        button.interactable = true;
        Color c = button.image.color;
        c.a = 1f;
        button.image.color = c;
        button.GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1, 1f);
    }

    void Update()
    {
        if (!button.interactable && Player.Instance.isReady())
        {
            activate();
        }
    }
}