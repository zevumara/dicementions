using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System;

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
    public int GetWeaponIndexByName(string name)
    {
        return Array.FindIndex(weaponPrefabs, prefab => prefab.name == name);
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

    public int GetEnemyIndexByName(string name)
    {
        return Array.FindIndex(enemyPrefabs, prefab => prefab.name == name);
    }

    public void StartSceneTransition(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        RectTransform imageTransform = fadeImage.GetComponent<RectTransform>();
        imageTransform.localPosition = new Vector3(imageTransform.localPosition.x, imageTransform.localPosition.y, -17339);
        yield return StartCoroutine(Fade(0f, 1f));

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Esperar a que esté cargada
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        asyncLoad.allowSceneActivation = true;

        yield return null;

        imageTransform.localPosition = new Vector3(imageTransform.localPosition.x, imageTransform.localPosition.y, 0f);
        fadeImage.color = new Color(0, 0, 0, 1);
        yield return StartCoroutine(Fade(1f, 0f));
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

        fadeImage.color = new Color(c.r, c.g, c.b, toAlpha);
    }
}