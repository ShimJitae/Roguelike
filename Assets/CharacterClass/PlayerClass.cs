using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerClass : Entity
{
    [SerializeField] float playerMaxHp = 100;
    [SerializeField] float playeratk = 10;
    [SerializeField] float playerdef= 10;
    [SerializeField] public int mobCount;
    private void Awake()
    {
        entityType = EntityType.Player;
        maxHp = playerMaxHp;
        hp = maxHp;
        atk = playeratk;
        def= playerdef;
        mobCount = 0;

    }
}
