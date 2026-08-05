using UnityEngine;
using UnityEngine.UI;

public class TitleSceneSetter : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;

    void Start()
    {
        SceneLoadManager slm = SceneLoadManager.Instance;
        startButton.onClick.AddListener(() => slm.LoadScene(SceneType.LobbyScene));
        exitButton.onClick.AddListener(() => slm.ExitGame());
    }
}
