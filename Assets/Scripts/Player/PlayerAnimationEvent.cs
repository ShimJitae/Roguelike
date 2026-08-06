using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerAnimationEvent : CharacterAnimationEvent
{
    [SerializeField] private PlayerController player;

    protected override void SpawnArrow()
    {
        base.SpawnArrow();
    }
    
    protected override void spawnSword()
    {
        base.spawnSword();

    }
    protected override void RemoveSword()
    {
        base.RemoveSword();
    }

    protected void OnDeath()
    {
        //TODO 로비씬으로 화면 전환 - 페이드인 페이드아웃
        // 그리고 플레이어 초기화
        SceneLoadManager.Instance.LoadScene("TitleScene");
    }
}