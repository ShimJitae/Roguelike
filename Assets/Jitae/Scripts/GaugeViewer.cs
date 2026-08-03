using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GaugeViewer : MonoBehaviour
{
    private Slider slider;

    [Header("Animation")]
    // 증가할 때와 감소할 때의 애니메이션 지속 시간을 따로 설정함으로써,
    // 게이지가 증가할 때는 부드럽게, 감소할 때는 빠르게 반응하도록 합니다.
    [SerializeField] private float increaseDuration = 1.75f;
    [SerializeField] private float decreaseDuration = 0.5f;

    // 현재 Tween을 추적하기 위한 변수입니다. 이를 통해 새로운 값이 설정될 때 기존 애니메이션을 중지하고 새로운 애니메이션을 시작할 수 있습니다.
    private Tween currentTween;
    private float previousValue;

    private void Awake()
    {
        // 인스펙터에서 연결하지 않았다면 현재 오브젝트에서 가져옵니다.
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        slider.value = slider.maxValue;
        previousValue = slider.value;

        SetMaxValue(100);
    }

    public void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
        previousValue = maxValue;
    }

    public void SetGauge(float newValue)
    {
        // Slider의 범위를 벗어나지 않도록 제한합니다.
        newValue = Mathf.Clamp(
            newValue,
            slider.minValue,
            slider.maxValue
        );

        // 이전에 저장된 값과 새롭게 할당해야 하는 값을 비교하여,
        // 새로운 값이 더 크면, 증가하는 애니메이션을
        // 그렇지 않다면 감소하는 애니메이션을 적용시켜줍니다.
        bool isIncreasing = newValue > previousValue;

        // 기존 게이지 애니메이션이 남아 있으면 중지합니다.
        currentTween?.Kill();

        currentTween = slider
        // newValue : 슬라이더의 목표값
            .DOValue(
                newValue,
        // isIncreasing 여부에 따라서, duration 값을 다르게 설정함
                isIncreasing ? increaseDuration : decreaseDuration
            )
            // Tween의 easing을 설정합니다.
            // Ease.OutCubic : 증가할 때 부드럽게, Ease.Linear : 감소할 때 빠르게 반응
            .SetEase(
                isIncreasing ? Ease.OutCubic : Ease.Linear
            )
            // Tween이 완료되었을 때 currentTween 변수를 null로 설정하여, 다음 애니메이션이 시작될 때 이전 Tween이 남아있지 않도록 합니다.
            .OnComplete(() =>
            {
                currentTween = null;
            });

        previousValue = newValue;
    }

    private void OnDisable()
    {
        currentTween?.Kill();
        currentTween = null;
    }


    // 게이지 변경 테스트용 메서드
    [SerializeField] float testValue = 50f;
    [ContextMenu("Test Set Gauge")]
    private void TestSetGauge()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Play 모드에서 실행해 주세요.");
            return;
        }

        SetGauge(testValue);
    }
}
