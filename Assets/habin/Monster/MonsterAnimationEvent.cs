using UnityEngine;

public class MonsterAnimationEvent : CharacterAnimationEvent
{
    private MonsterMove monsterMove;
    private ItemDrop itemDrop;
    private PlayerClass playerClass;
    private void Start()
    {
        monsterMove = GetComponentInParent<MonsterMove>();
        itemDrop = GetComponentInParent<ItemDrop>();
        playerClass = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerClass>();
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
        playerClass.mobCount++;
        Destroy(transform.parent.gameObject);
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
