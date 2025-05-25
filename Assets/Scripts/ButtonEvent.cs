using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public void OnStartClicked()
    {
        string randomLevel = GameManager.Instance.GetRandomLevel();
        GameManager.Instance.StartSceneTransition(randomLevel);
    }
    public void OnReplayClicked()
    {
        Player.Instance.Reset();
        GameManager.Instance.StartSceneTransition("Dices");
    }
}