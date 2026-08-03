using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Monster Data")]
public class MonsterData : ScriptableObject
{
    [SerializeField] private string monsterId;
    [SerializeField] private string monsterName;
    [SerializeField] private string monsterMaxHp;
    [SerializeField] private string monsterAttack;
    [SerializeField] private string monsterBodyAttack;
    [SerializeField] private string monsterDefense;
    [SerializeField] private string monsterExp;
    private bool monsterAlive {  get; set; }


}
