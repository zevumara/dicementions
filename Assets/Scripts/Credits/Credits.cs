using UnityEngine;
[System.Obsolete]
public class Credits: MonoBehaviour
{
    public GameObject credits;
    public void Test()
    {
        Debug.Log("funcionó pa");
    }

    public void ToggleCredits()
    {
        credits.SetActive(!credits.active);
    }
}
