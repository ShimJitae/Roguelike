using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance { get; private set; }

    public event Action OnSetUpScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        FadeManager.Instance.OnFadeComplete += () => SceneManager.LoadScene(sceneName);
        FadeManager.Instance.Fade();

        Debug.Log($"활성화된 씬 : {SceneManager.GetActiveScene().name}");
    }

    void SetUpMinimap()
    {

    }
}