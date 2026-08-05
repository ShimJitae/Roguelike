using System;
using DG.Tweening;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private FadeUI fadeUI;

    public static FadeManager Instance { get; private set; }

    public bool IsFading { get; private set; }

    // FadeOut과 FadeIn까지 모두 끝난 시점
    public event Action OnFadeComplete;

    private Tween currentTween;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeUI == null)
        {
            Debug.LogError("FadeManager에 FadeUI가 연결되지 않았습니다.", this);
            enabled = false;
            return;
        }

        fadeUI.ResetToTransparent();
        fadeUI.gameObject.SetActive(false);
    }

    /// <param name="onScreenCovered">
    /// 화면이 완전히 검어진 순간 실행할 작업.
    /// 일반적으로 씬 로드를 전달한다.
    /// </param>
    public void Fade(Action onScreenCovered = null)
    {
        if (IsFading || fadeUI == null)
            return;

        IsFading = true;

        fadeUI.gameObject.SetActive(true);
        fadeUI.ResetToTransparent();

        // 1. 현재 화면을 검게 가린다.
        currentTween = fadeUI.FadeOut()
            .OnComplete(() =>
            {
                // 2. 완전히 검어진 상태에서 이벤트를 실행시킨다.
                onScreenCovered?.Invoke();

                // 3. 이후 화면을 다시 투명하게 만든다.
                currentTween = fadeUI.FadeIn()
                    .OnComplete(FinishFade);
            });
    }

    private void FinishFade()
    {
        currentTween = null;
        fadeUI.gameObject.SetActive(false);
        IsFading = false;

        OnFadeComplete?.Invoke();
    }

    private void OnDestroy()
    {
        currentTween?.Kill();

        if (Instance == this)
            Instance = null;
    }
}