using DG.Tweening;
using UnityEngine;

public sealed class WeaponPanel : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private RectTransform slot1;
    [SerializeField] private RectTransform slot2;

    [Header("Animation")]
    [SerializeField] private float swapDuration = 0.35f;
    [SerializeField] private float arcHeight = 20f;

    private Vector2 slot1Position;
    private Vector2 slot2Position;

    private WeaponSlot slot1WeaponSlot;
    private WeaponSlot slot2WeaponSlot;

    private Tween swapTween;
    private bool isSwapped;

    private void Awake()
    {
        if (slot1 == null || slot2 == null)
        {
            Debug.LogError("WeaponPanel: Slot references are not assigned.", this);
            enabled = false;
            return;
        }

        slot1WeaponSlot = slot1.GetComponent<WeaponSlot>();
        slot2WeaponSlot = slot2.GetComponent<WeaponSlot>();

        if (slot1WeaponSlot == null || slot2WeaponSlot == null)
        {
            Debug.LogError("WeaponPanel: WeaponSlot components are missing.", this);
            enabled = false;
            return;
        }

        slot1Position = slot1.anchoredPosition;
        slot2Position = slot2.anchoredPosition;

        ApplySlotState();
    }

    [ContextMenu("Test Swap Equipment")]
    public void SwapEquipment()
    {
        isSwapped = !isSwapped;

        swapTween?.Kill();
        swapTween = null;

        Vector2 slot1Start = slot1.anchoredPosition;
        Vector2 slot2Start = slot2.anchoredPosition;
        Vector2 slot1End = isSwapped ? slot2Position : slot1Position;
        Vector2 slot2End = isSwapped ? slot1Position : slot2Position;

        ApplySlotState();

        if (slot1Start == slot1End && slot2Start == slot2End)
        {
            return;
        }

        Vector2 slot1Control = GetControlPoint(slot1Start, slot1End);
        Vector2 slot2Control = GetControlPoint(slot2Start, slot2End);

        swapTween = DOVirtual.Float(0f, 1f, swapDuration, progress =>
            {
                slot1.anchoredPosition = GetBezierPoint(
                    slot1Start,
                    slot1Control,
                    slot1End,
                    progress
                );

                slot2.anchoredPosition = GetBezierPoint(
                    slot2Start,
                    slot2Control,
                    slot2End,
                    progress
                );
            })
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                swapTween = null;
            });
    }

    private void ApplySlotState()
    {
        RectTransform frontSlot = isSwapped ? slot2 : slot1;
        WeaponSlot frontWeaponSlot = isSwapped ? slot2WeaponSlot : slot1WeaponSlot;
        WeaponSlot backWeaponSlot = isSwapped ? slot1WeaponSlot : slot2WeaponSlot;

        frontSlot.SetAsLastSibling();
        frontWeaponSlot.ActiveShadow(false);
        backWeaponSlot.ActiveShadow(true);
    }

    private Vector2 GetControlPoint(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized;

        return (start + end) * 0.5f + perpendicular * arcHeight;
    }

    private Vector2 GetBezierPoint(
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
        swapTween?.Kill();
        swapTween = null;
    }
}
