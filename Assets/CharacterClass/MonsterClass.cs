using UnityEngine;

public class MonsterClass : Entity
{
    private CharacterAnimController ancon;
    [SerializeField] private MonsterData monsterData;
    private string monsterId;
    private string monsterName;
    protected float monsterBodyAttack;
    public float MonsterBodyAttack => monsterBodyAttack;
    private float monsterExp;
    protected override void Awake()
    {
        base.Awake();

        entityType = EntityType.Monster;
        ancon = GetComponent<CharacterAnimController>();
        maxHp = monsterData.MaxHp;
        hp = maxHp;
        atk = monsterData.Attack;
        def = monsterData.Defense;
        monsterId = monsterData.MonsterId;
        attackType = monsterData.AttackType;
        monsterName = monsterData.MonsterName;
        monsterBodyAttack = monsterData.BodyAttack;
        monsterExp = monsterData.Exp;
        money = monsterData.Money;
    }
    public void PlayerAttack()
    {
        Debug.Log("MonsterAttack!");
        if (attackType == MonsterAttackType.ranged)
        {
            ancon.PlayAttackBow();
            return;
        }
        else if (attackType == MonsterAttackType.melee)
        {
            ancon.PlayAttack();
        }
    }
}
