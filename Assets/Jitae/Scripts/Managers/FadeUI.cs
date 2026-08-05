using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField, Min(0f)] private float duration = 1f;

    private void Awake()
    {
        ResetToTransparent();
    }

    // 투명한 화면 → 검은 화면
    public Tween FadeOut()
    {
        return Fade(1f);
    }

    // 검은 화면 → 투명한 화면
    public Tween FadeIn()
    {
        return Fade(0f);
    }

    public void ResetToTransparent()
    {
        fadeImage.DOKill();
        fadeImage.color = new Color(0f, 0f, 0f, 0f);
    }

    private Tween Fade(float targetAlpha)
    {
        return fadeImage
            .DOFade(targetAlpha, duration)
            .SetEase(Ease.Linear)
            .SetUpdate(true);
    }
}