using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject[] weaponPrefabs;
    public GameObject[] enemyPrefabs;
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetWeaponByName(string name)
    {
        return weaponPrefabs.FirstOrDefault(w => w.name == name);
    }

    public GameObject GetWeaponByIndex(int index)
    {
        if (index >= 0 && index < weaponPrefabs.Length)
        {
            return weaponPrefabs[index];
        }
        else
        {
            Debug.LogWarning("Índice de arma fuera de rango.");
            return null;
        }
    }

    public GameObject GetEnemyByName(string name)
    {
        return enemyPrefabs.FirstOrDefault(w => w.name == name);
    }

    public GameObject GetEnemyByIndex(int index)
    {
        if (index >= 0 && index < enemyPrefabs.Length)
        {
            return enemyPrefabs[index];
        }
        else
        {
            Debug.LogWarning("Índice de enemigo fuera de rango.");
            return null;
        }
    }

    public void StartSceneTransition(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // FADE IN
        yield return StartCoroutine(Fade(0f, 1f));

        // CARGA ASÍNCRONA
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Esperá a terminar el fade antes de mostrar

        // Esperar a que esté cargada
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // FADE OUT OPCIONAL (o solo activar la escena)
        yield return new WaitForSeconds(0.2f); // Pequeño delay si querés
        asyncLoad.allowSceneActivation = true;
    }

    private IEnumerator Fade(float fromAlpha, float toAlpha)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        // Asegurar el valor final
        fadeImage.color = new Color(c.r, c.g, c.b, toAlpha);
    }
}