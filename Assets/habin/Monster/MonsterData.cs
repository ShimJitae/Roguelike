using UnityEngine;
public enum MonsterAttackType
{
    melee,//근접
    ranged,//원거리
    Boss

}
[CreateAssetMenu(menuName = "Game Data/Monster Data")]
public class MonsterData : ScriptableObject
{
    [SerializeField] private string monsterId;
    [SerializeField] private MonsterAttackType attackType;
    [SerializeField] private string monsterName;
    [SerializeField] private float monsterMaxHp;
    [SerializeField] private float monsterAttack;
    [SerializeField] private float monsterBodyAttack;
    [SerializeField] private float monsterDefense;
    [SerializeField] private float monsterExp;
    public string MonsterId => monsterId;
    public MonsterAttackType AttackType => attackType;
    public string MonsterName => monsterName;
    public float MaxHp => monsterMaxHp;
    public float Attack => monsterAttack;
    public float BodyAttack => monsterBodyAttack;
    public float Defense => monsterDefense;
    public float Exp => monsterExp;


}
