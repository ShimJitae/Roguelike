using DG.Tweening;
using UnityEngine;

public class WeaponPanel : MonoBehaviour
{
    // 각 무기의 슬롯
    [Header("Slots")]
    [SerializeField] private RectTransform slot1;
    [SerializeField] private RectTransform slot2;

    // 무기가 스왑되는 애니메이션
    [Header("Animation")]
    [SerializeField, Min(0.01f)]
    private float swapDuration = 0.35f;

    // 두 슬롯이 겹치지 않게 휘어지는 정도
    [SerializeField]
    private float arcHeight = 20f;

    // 일시정지 상태에서도 교체 애니메이션을 실행할지
    [SerializeField]
    private bool ignoreTimeScale = false;

    private Vector2 slot1StartPosition;
    private Vector2 slot2StartPosition;

    private Sequence swapSequence;

    private bool isSwapped;
    // 사용자가 마지막으로 요청한 상태
    private bool targetSwapped;

    private void Awake()
    {
        slot1StartPosition = slot1.anchoredPosition;
        slot2StartPosition = slot2.anchoredPosition;
    }

    public void SwapEquipment()
    {
        // 입력할 때마다 목표 상태를 즉시 반전
        targetSwapped = !targetSwapped;

        // 이전 애니메이션은 현재 위치에서 중단
        swapSequence?.Kill();

        Vector2 slot1From = slot1.anchoredPosition;
        Vector2 slot2From = slot2.anchoredPosition;

        Vector2 slot1Target = targetSwapped
            ? slot2StartPosition
            : slot1StartPosition;

        Vector2 slot2Target = targetSwapped
            ? slot1StartPosition
            : slot2StartPosition;

        // 남은 거리에 따라 재생 시간 조절
        float fullDistance = Vector2.Distance(slot1StartPosition, slot2StartPosition);

        float remainingDistance = Vector2.Distance(slot1From, slot1Target);

        float distanceRatio = remainingDistance / Mathf.Max(fullDistance, 0.001f);

        float currentDuration = Mathf.Max(0.08f, swapDuration * distanceRatio);

        Vector2 middle = (slot1From + slot1Target) * 0.5f;

        Vector2 direction = slot1Target - slot1From;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized;

        float currentArcHeight = arcHeight * distanceRatio;

        Vector2 slot1Control = middle + perpendicular * currentArcHeight;

        Vector2 slot2Control = middle - perpendicular * currentArcHeight;

        bool completedState = targetSwapped;

        swapSequence = DOTween.Sequence();

        swapSequence.Join(
            CreateArcTween(slot1, slot1From, slot1Control, slot1Target, currentDuration)
        );

        swapSequence.Join(
            CreateArcTween(slot2, slot2From, slot2Control, slot2Target, currentDuration)
        );

        swapSequence
            .SetUpdate(ignoreTimeScale)
            .OnComplete(() =>
            {
                slot1.anchoredPosition = slot1Target;
                slot2.anchoredPosition = slot2Target;

                isSwapped = completedState;
                swapSequence = null;
            });
    }

    private Tween CreateArcTween(RectTransform target, Vector2 start, Vector2 control, Vector2 end, float duration)
    {
        return DOVirtual.Float(
                0f,
                1f,
                duration,
                progress =>
                {
                    target.anchoredPosition = CalculateBezierPoint(start, control, end, progress);
                })
            .SetEase(Ease.InOutSine);
    }

    // 2차 베지어 곡선
    private static Vector2 CalculateBezierPoint(Vector2 start, Vector2 control, Vector2 end, float progress)
    {
        float reverse = 1f - progress;

        return reverse * reverse * start
             + 2f * reverse * progress * control
             + progress * progress * end;
    }

    private void OnDisable()
    {
        swapSequence?.Kill();
        swapSequence = null;

        // 애니메이션 중 비활성화됐다면 원래 상태로 정렬
        slot1.anchoredPosition = isSwapped
            ? slot2StartPosition
            : slot1StartPosition;

        slot2.anchoredPosition = isSwapped
            ? slot1StartPosition
            : slot2StartPosition;
    }
}