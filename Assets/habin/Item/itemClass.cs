using UnityEngine;

public class itemClass : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
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
    }


}
