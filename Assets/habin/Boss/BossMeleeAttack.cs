using UnityEngine;

public class BossMeleeAttack : MonoBehaviour
{
    private float swordDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void SetDamage(float attack)
    {
        swordDamage = attack;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerClass playerClass = other.GetComponent<PlayerClass>();

        if (playerClass != null)
        {
            playerClass.OnHit?.Invoke(Mathf.Max(0, swordDamage - playerClass.Def));
            Debug.Log($"남은 채력{playerClass.Hp}");
        }

    }
}
