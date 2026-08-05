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
        Debug.Log("검 이펙트 스폰");

    }
    protected override void RemoveSword()
    {
        Debug.Log("검 이펙트 제거");
    }
}
