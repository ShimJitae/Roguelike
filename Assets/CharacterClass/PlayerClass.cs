using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerClass : Entity
{
    [SerializeField] float playerMaxHp = 100;
    [SerializeField] float playeratk = 10;
    [SerializeField] float playerdef= 10;
    [SerializeField] public int mobCount;
    [SerializeField] public int attackUpgradeLevel = 1;
    [SerializeField] public int defenseUpgradeLevel = 1;
    private void Awake()
    {
        entityType = EntityType.Player;
        maxHp = playerMaxHp;
        hp = maxHp;
        atk = playeratk;
        def= playerdef;
        mobCount = 0;

    }
    public void UseMoney(int value)
    {
        money -= value;
        if (money < 0)
        {
            money = 0;
        }
    }
    public void AddAttack(float value)
    {
        atk += value;
    }
    public void AddDefense(float value)
    {
        def += value;
    }

}
