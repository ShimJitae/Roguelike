using UnityEngine;

public class MonsterAnimationEvent : CharacterAnimationEvent
{
    private MonsterMove monsterMove;
    private ItemDrop itemDrop;
    private void Start()
    {
        monsterMove = GetComponentInParent<MonsterMove>();
        itemDrop = GetComponentInParent<ItemDrop>();
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
    protected override void OnDeath()
    {
        Destroy(transform.parent);
    }

    protected override void DropItem()
    {
        itemDrop.DropItem();
    }


    public void EndAttack()
    {
        monsterMove.isAttacking = false;
    }
    
}
