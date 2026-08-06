using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private MonsterClass monsterClass;
    [Header("드랍 아이템")]
    [SerializeField] private GameObject magicStone;
    [SerializeField] private GameObject dropItem1;
    [SerializeField] private GameObject dropItem2;
    [SerializeField] private GameObject dropItem3;
    [SerializeField] private GameObject dropItem4;
    [SerializeField, Range(0f, 1f)] private float dropChance = 0.3f;

    public void Awake()
    {
        monsterClass = GetComponent<MonsterClass>();
    }
    public void DropItem()
    {
        Vector3 dropPosition = transform.position + Vector3.up * 0.5f;

        if (magicStone != null)
        {
            GameObject stone = Instantiate(magicStone, dropPosition, Quaternion.identity);
            //생성시킨 몬스터? 드롭아이템을 실행시킨 몬스터의 데이터 값을 받아와야함

            itemClass stoneItemClass = stone.GetComponent<itemClass>();
            if (stoneItemClass != null)
            {
                stoneItemClass.SetMoney(monsterClass.Money);
            }

        }
        if (dropItem1 != null )
        {
            if (Random.value <= dropChance)
            {
                Instantiate(dropItem1, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }

        }
        if(dropItem2 != null)
        {
            if (Random.value <= dropChance)
            {
                Instantiate(dropItem2, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }

        }
        if(dropItem3 != null)
        {
            if (Random.value <= dropChance)
            {
                Instantiate(dropItem3, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }

        }
        if(dropItem4 != null)
        {
            if (Random.value <= dropChance)
            {
                Instantiate(dropItem4, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }

        }

    }
}
