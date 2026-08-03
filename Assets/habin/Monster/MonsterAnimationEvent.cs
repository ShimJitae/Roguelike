using UnityEngine;

public class MonsterAnimationEvent : MonoBehaviour
{
    private MonsterMove monsterMove;
    private void Awake()
    {
        monsterMove = GetComponentInParent<MonsterMove>();
    }
    public void SpawnArrow()
    {
        if (monsterMove != null)
        {
            monsterMove.SpawnArrow();
        }
    }
    public void spawnSword()
    {
        if (monsterMove != null)
        {
            monsterMove.SpawnSword();
        }

    }
    public void RemoveSword()
    {
        if (monsterMove != null)
        {
            monsterMove.RemoveSword();
        }

    }
}
