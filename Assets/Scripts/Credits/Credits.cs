using UnityEngine;
[System.Obsolete]
public class Credits: MonoBehaviour
{
    public GameObject credits;
    public void Test()
    {
        Debug.Log("funcion� pa");
    }
    public void GoToLink(string link)
    {
        Debug.Log(link);
        Application.OpenURL(link);
    }

    public void ToggleCredits()
    {
        credits.SetActive(!credits.active);
    }
}
