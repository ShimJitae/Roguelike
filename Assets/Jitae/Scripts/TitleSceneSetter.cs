using UnityEngine;
using UnityEngine.UI;

public class TitleSceneSetter : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;

    void Start()
    {
        SceneLoadManager slm = SceneLoadManager.Instance;
        startButton.onClick.AddListener(() => slm.LoadScene("DungeonScene"));
        exitButton.onClick.AddListener(() => slm.ExitGame());
    }
}
