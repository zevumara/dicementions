using System.Collections.Generic;
using UnityEngine;

public class HitPointsUI : MonoBehaviour
{
    public GameObject heartObject;
    public Player player;
    private List<HeartUI> hearts = new List<HeartUI>();

    private void OnEnable()
    {
        Player.onPlayerDamaged += DrawHearts;
    }

    private void OnDisable()
    {
        Player.onPlayerDamaged -= DrawHearts;
    }

    private void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();
        float maxHpRemainder = player.maxHp % 2;
        int heartsToMake = (int) ((player.maxHp / 2) + maxHpRemainder);
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }
        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int) Mathf.Clamp(player.hp - (i*2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus) heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartObject);
        newHeart.transform.SetParent(transform, false);
        HeartUI heartComponent = newHeart.GetComponent<HeartUI>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HeartUI>();
    }
}
