using UnityEngine;
[System.Obsolete]
public class Credits: MonoBehaviour
{
    public GameObject credits;
    public void Test()
    {
        Debug.Log("funcion� pa");
    }

    public void ToggleCredits()
    {
        credits.SetActive(!credits.active);
    }
}
