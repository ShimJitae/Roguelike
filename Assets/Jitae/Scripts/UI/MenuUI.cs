using System;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    // 각 버튼 별 이벤트를 외부에서 구독할 수 있도록 Action 이벤트를 정의합니다.
    public event Action OnResume;
    public event Action OnRestart;
    public event Action OnGoTitle;

    // 현재 진행 중인 게임 재개
    public void B_Resume()
    {
        OnResume?.Invoke();
    }

    // 새로운 게임 시작
    public void B_Restart()
    {
        OnRestart?.Invoke();
    }

    // 타이틀 화면으로 이동
    public void B_GoTitle()
    {
        OnGoTitle?.Invoke();
    }
}
