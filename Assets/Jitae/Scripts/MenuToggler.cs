using UnityEngine;

public class MenuToggler : MonoBehaviour
{
    [SerializeField] GameObject menuCanvasPrefab;

    GameObject menuCanvas;

    private void Awake()
    {
        menuCanvas = Instantiate(menuCanvasPrefab, transform);
        menuCanvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        MenuUI menuUI = menuCanvas.GetComponent<MenuUI>();

        menuUI.OnResume += () => menuCanvas.SetActive(false);

        menuUI.OnRestart += () => SceneLoadManager.Instance.LoadScene(SceneType.LobbyScene);

        menuUI.OnGoTitle += () => SceneLoadManager.Instance.LoadScene(SceneType.TitleScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        if (menuCanvas.activeSelf)
        {
            menuCanvas.SetActive(false);
        }
        else
        {
            menuCanvas.SetActive(true);
        }
    }
}
