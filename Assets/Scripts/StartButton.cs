using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Player player;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        Color c = button.image.color;
        c.a = 0.1f;
        button.image.color = c;
        button.GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1, 0.1f);
    }

    void Update()
    {
        if (player.isReady)
        {
            button.interactable = true;
            Color c = button.image.color;
            c.a = 1f;
            button.image.color = c;
            button.GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1, 1f);
        }
        else
        {
            button.interactable = false;
        }
    }
}