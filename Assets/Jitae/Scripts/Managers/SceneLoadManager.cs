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

    public void LoadScene(SceneType sceneType)
    {
        LoadScene(sceneType.ToString());
    }

    public void LoadScene(string sceneName)
    {
        SoundManager.Instance.StopBGM();
        FadeManager.Instance.OnFadeOutComplete += () => SceneManager.LoadScene(sceneName);
        FadeManager.Instance.OnFadeOutComplete += () => PlayBGMBySceneName(sceneName);
        FadeManager.Instance.Fade();

        Debug.Log($"활성화된 씬 : {SceneManager.GetActiveScene().name}");
    }

    private void PlayBGMBySceneName(string sceneName)
    {
        switch (sceneName)
        {
            case "LobbyScene":
            case "DungeonScene": SoundManager.Instance.PlayBGM(BGMType.Normal); break;
            case "TitleScene": SoundManager.Instance.PlayBGM(BGMType.Title); break;
        }
    }

    public void ExitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
{
    
    Application.Quit();
}
    
#endif
    }
}