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
}
