using UnityEngine;

public class PlayerClass : CharacterClass
{
    [SerializeField] float playerMaxHp = 100;
    [SerializeField] float playerattack = 10;
    [SerializeField] float playerdefense = 10;
    public float hp;
    private void Awake()
    {
        maxHp = playerMaxHp;
        hp = maxHp;
        attack = playerattack;
        defense = playerdefense;

    }
}
