using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class BossHPBarEnabler : MonoBehaviour
{
    [SerializeField] GameObject bossUIPrefab;
    GameObject bossUI;
    Slider bossHPBar;

    Vector2 hpBarStartPos, hpBarEndPos;

    public void SetBossHPMaxValue(float maxValue)
    {
        GetComponent<GaugeViewer>().SetSliderMaxValue(maxValue);
    }

    void OnEnable()
    {
        bossUI = Instantiate(bossUIPrefab);
        StartCoroutine(EnableBossHPBar());
    }

    void OnDisable()
    {
        Destroy(bossUI);
    }

    [SerializeField] float tweenDuration = 1.5f;
    [ContextMenu("Test EnableBossHPBar")]
    public IEnumerator EnableBossHPBar()
    {
        yield return new WaitForSeconds(2.5f);

        SetEnabler();
        bossUI.SetActive(true);

        bossHPBar.GetComponent<RectTransform>().anchoredPosition = hpBarStartPos;
        bossHPBar.GetComponent<RectTransform>().DOAnchorPos(hpBarEndPos, tweenDuration).SetEase(Ease.OutCubic);
    }

    private void SetEnabler()
    {
        bossHPBar = bossUI.GetComponentInChildren<Slider>();
        bossHPBar.onValueChanged.AddListener(_ => DestroyWhenValue0());

        bossHPBar.gameObject.SetActive(true);
        bossUI.gameObject.SetActive(false);

        GaugeViewer gv = GetComponent<GaugeViewer>();
        if (gv == null)
        {
            Debug.LogError("해당 오브젝트에 GaugeViewer 컴포넌트가 없습니다.");
            return;
        }

        gv.Slider = bossHPBar;

        hpBarStartPos = bossHPBar.GetComponent<RectTransform>().anchoredPosition;
        hpBarEndPos = new Vector2(hpBarStartPos.x, hpBarStartPos.y - 180f);
    }

    void DestroyWhenValue0()
    {
        if (bossHPBar.value <= 0)
        {
            Destroy(bossUI);
        }
    }
}
