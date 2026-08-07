using UnityEngine;
using UnityEngine.UI;

public class TitleSceneSetter : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;

    void Start()
    {
        Destroy(GameObject.FindWithTag("Player"));

        SceneLoadManager slm = SceneLoadManager.Instance;
        startButton.onClick.AddListener(() => slm.LoadScene("LobbyScene"));
        exitButton.onClick.AddListener(() => slm.ExitGame());
    }
}
