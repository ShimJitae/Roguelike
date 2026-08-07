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
        Vector3 dropPosition1 = transform.position + Vector3.up * 0.5f + Vector3.left * 0.5f; ;
        Vector3 dropPosition2 = transform.position + Vector3.up * 0.5f + Vector3.right * 0.5f; ;
        Vector3 dropPosition3 = transform.position + Vector3.up * 0.5f + Vector3.left * 1f; ;
        Vector3 dropPosition4 = transform.position + Vector3.up * 0.5f + Vector3.right * 1f; ;

        if (magicStone != null)
        {
            GameObject stone = Instantiate(magicStone, dropPosition, Quaternion.identity);

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
                Instantiate(dropItem1, dropPosition1, Quaternion.identity);
            }

        }
        if(dropItem2 != null)
        {
            if (Random.value <= dropChance)
            {
                Instantiate(dropItem2, dropPosition2, Quaternion.identity);
            }

        }
        if(dropItem3 != null)
        {
            if (Random.value <= dropChance)
            {
                Instantiate(dropItem3, dropPosition3, Quaternion.identity);
            }

        }
        if(dropItem4 != null)
        {
            if (Random.value <= dropChance)
            {
                Instantiate(dropItem4, dropPosition4, Quaternion.identity);
            }

        }

    }
}
