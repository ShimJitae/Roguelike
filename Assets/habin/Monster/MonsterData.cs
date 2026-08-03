using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Monster Data")]
public class MonsterData : ScriptableObject
{
    [SerializeField] private string monsterId;
    [SerializeField] private string monsterName;
    [SerializeField] private float monsterMaxHp;
    [SerializeField] private float monsterAttack;
    [SerializeField] private float monsterBodyAttack;
    [SerializeField] private float monsterDefense;
    [SerializeField] private float monsterExp;
    private bool monsterAlive {  get; set; }


}
