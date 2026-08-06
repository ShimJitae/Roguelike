using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [Header("드랍 아이템")]
    [SerializeField] private GameObject dropItem;
    [SerializeField, Range(0f, 1f)] private float dropChance = 0.3f;

    public void DropItem()
    {
        if (Random.value <= dropChance)
        {
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
    }
}
