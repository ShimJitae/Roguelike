using System;
using DG.Tweening;
using UnityEngine;

/*
FadeManager 사용법

화면에 FadeOut 되었을 때, 실행시키고 싶은 이벤트를 먼저 OnFadeComplete에 등록해줍니다.
FadeManager.Instance.OnFadeComplete += () => { 이벤트 실행 }

Fade()를 실행시킵니다.
FadeManager.Instance.Fade()
*/

public class FadeManager : MonoBehaviour
{
    [SerializeField] private FadeUI fadeUI;

    public static FadeManager Instance { get; private set; }

    public bool IsFading { get; private set; }

    // FadeOut이 끝난 시점
    public event Action OnFadeOutComplete;

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
            Debug.LogError("FadeManager에 FadeUI가 연결되지 않았습니다.");
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
    public void Fade()
    {
        if (IsFading || fadeUI == null)
            return;

        IsFading = true;

        SoundManager.Instance.StopBGM();

        fadeUI.gameObject.SetActive(true);
        fadeUI.ResetToTransparent();

        // 1. 현재 화면을 검게 가린다.
        currentTween = fadeUI.FadeOut()
            .OnComplete(() =>
            {
                // 2. 완전히 검어진 상태에서 이벤트를 실행시킨다.
                OnFadeOutComplete?.Invoke();

                // 3. 1초 대기 후 화면을 다시 투명하게 만든다.
                currentTween = DOVirtual.DelayedCall(1f, () =>
                {
                    currentTween = fadeUI.FadeIn()
                        .OnComplete(FinishFade);
                });
            });
    }

    private void FinishFade()
    {
        currentTween = null;
        fadeUI.gameObject.SetActive(false);
        IsFading = false;
        OnFadeOutComplete = null;
    }

    private void OnDestroy()
    {
        currentTween?.Kill();

        if (Instance == this)
            Instance = null;
    }
}
