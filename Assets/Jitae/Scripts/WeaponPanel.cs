using DG.Tweening;
using UnityEngine;

public sealed class WeaponPanel : MonoBehaviour
{
    private const float MinimumDuration = 0.08f;
    private const float PositionEpsilon = 0.001f;

    [Header("Slots")]
    [SerializeField] private RectTransform slot1;
    [SerializeField] private RectTransform slot2;

    [Header("Animation")]
    [SerializeField, Min(0.01f)] private float swapDuration = 0.35f;
    [SerializeField] private float arcHeight = 20f;
    [SerializeField] private bool ignoreTimeScale;

    private Vector2 slot1Position;
    private Vector2 slot2Position;
    private Tween swapTween;
    private bool isSwapped;
    private bool isInitialized;

    private void Awake()
    {
        if (slot1 == null || slot2 == null)
        {
            Debug.LogError("WeaponPanel: Slot references are not assigned.", this);
            enabled = false;
            return;
        }

        slot1Position = slot1.anchoredPosition;
        slot2Position = slot2.anchoredPosition;
        isInitialized = true;

        ApplyState(false);
    }

    [ContextMenu("Test Swap Equipment")]
    public void SwapEquipment()
    {
        if (!isInitialized)
        {
            return;
        }

        isSwapped = !isSwapped;
        KillSwapTween();

        Vector2 slot1Start = slot1.anchoredPosition;
        Vector2 slot2Start = slot2.anchoredPosition;
        Vector2 slot1End = isSwapped ? slot2Position : slot1Position;
        Vector2 slot2End = isSwapped ? slot1Position : slot2Position;

        float fullDistance = Vector2.Distance(slot1Position, slot2Position);
        float remainingDistance = Mathf.Max(
            Vector2.Distance(slot1Start, slot1End),
            Vector2.Distance(slot2Start, slot2End));

        if (fullDistance <= PositionEpsilon || remainingDistance <= PositionEpsilon)
        {
            ApplyState(isSwapped);
            return;
        }

        float distanceRatio = Mathf.Clamp01(remainingDistance / fullDistance);
        float duration = Mathf.Max(MinimumDuration, swapDuration * distanceRatio);
        float height = arcHeight * distanceRatio;

        Vector2 slot1Control = GetArcControlPoint(slot1Start, slot1End, height);
        Vector2 slot2Control = GetArcControlPoint(slot2Start, slot2End, height);

        // DOVirtual tweens should not be nested in a Sequence.
        // One progress tween updates both slots together instead.
        swapTween = DOVirtual.Float(0f, 1f, duration, progress =>
            {
                slot1.anchoredPosition = GetBezierPoint(
                    slot1Start,
                    slot1Control,
                    slot1End,
                    progress);

                slot2.anchoredPosition = GetBezierPoint(
                    slot2Start,
                    slot2Control,
                    slot2End,
                    progress);
            })
            .SetEase(Ease.InOutSine)
            .SetUpdate(UpdateType.Normal, ignoreTimeScale)
            .SetTarget(this)
            .SetAutoKill(true)
            .OnComplete(() => ApplyState(isSwapped))
            .OnKill(() => swapTween = null);
    }

    private void ApplyState(bool swapped)
    {
        slot1.anchoredPosition = swapped ? slot2Position : slot1Position;
        slot2.anchoredPosition = swapped ? slot1Position : slot2Position;

        if (swapped)
        {
            slot2.SetAsLastSibling();
        }
        else
        {
            slot1.SetAsLastSibling();
        }
    }

    private void KillSwapTween()
    {
        swapTween?.Kill();
        swapTween = null;
    }

    private static Vector2 GetArcControlPoint(
        Vector2 start,
        Vector2 end,
        float height)
    {
        Vector2 direction = end - start;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized;

        return (start + end) * 0.5f + perpendicular * height;
    }

    private static Vector2 GetBezierPoint(
        Vector2 start,
        Vector2 control,
        Vector2 end,
        float progress)
    {
        float inverse = 1f - progress;

        return inverse * inverse * start
             + 2f * inverse * progress * control
             + progress * progress * end;
    }

    private void OnDisable()
    {
        KillSwapTween();

        if (isInitialized)
        {
            ApplyState(isSwapped);
        }
    }
}
