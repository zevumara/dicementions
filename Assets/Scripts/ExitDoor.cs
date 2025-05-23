using System.Collections;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(HandleExitSequence(collision.GetComponent<Player>()));
        }
    }
    private IEnumerator HandleExitSequence(Player player)
    {
        LevelManager.Instance.countdownText.gameObject.SetActive(true);
        LevelManager.Instance.countdownText.text = "¡Sala superada!";
        player.FadeOut(1.5f);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.StartSceneTransition("Dice Scene");
    }
}
