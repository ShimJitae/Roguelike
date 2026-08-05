using UnityEngine;

public class MenuToggler : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas;

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
