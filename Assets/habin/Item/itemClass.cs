using UnityEngine;

public class itemClass : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    private int money;

    public void SetMoney(int value)
    {
        money = value;
    }
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        PlayerClass playerclass = collision.gameObject.GetComponent<PlayerClass>();
        if (itemData.ItemType == ItemType.Potion)
        {
            playerclass.EntityHeal(itemData.ItemHp);
            Debug.Log($"회복하였습니다 현재 HP {playerclass.Hp}");
            Destroy(gameObject);
            return;
        }
        if (itemData.ItemId == "000")
        {
            playerclass.UpMoney(money);
            Debug.Log($"마석을 획득하였습니다 현재 소지 마석 {playerclass.Money}");
            Destroy(gameObject);
            return;
        }
    }


}
