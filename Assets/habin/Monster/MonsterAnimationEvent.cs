using UnityEngine;

public class MonsterAnimationEvent : CharacterAnimationEvent
{
    private MonsterMove monsterMove;
    private void Start()
    {
        monsterMove = GetComponentInParent<MonsterMove>();
    }

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

    
    public void EndAttack()
    {
        monsterMove.isAttacking = false;
    }
    
}
