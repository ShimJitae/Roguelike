using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform portalMovePoint;
    [SerializeField] private int fildMobCount;
    private PlayerClass playerClass;
    private SpriteRenderer sp;

    [SerializeField] GameObject bossMob;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        playerClass = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerClass>();
    }
    private void Update()
    {
        if (playerClass.mobCount < fildMobCount)
        {
            sp.enabled = false;
            return;
        }
        if (playerClass.mobCount >= fildMobCount)
        {
            sp.enabled = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || playerClass.mobCount < fildMobCount)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (playerClass.mobCount == 1)
            {
                FadeManager.Instance.OnFadeOutComplete += () => bossMob.gameObject.SetActive(false);
                SceneLoadManager.Instance.LoadScene("TitleScene");
            }
            else
            {
                FadeManager.Instance.OnFadeOutComplete += () => other.transform.position = portalMovePoint.position;
                FadeManager.Instance.OnFadeOutComplete += () => bossMob.gameObject.SetActive(true);
                FadeManager.Instance.Fade();
            }
            playerClass.mobCount = 0;
        }
    }



}
