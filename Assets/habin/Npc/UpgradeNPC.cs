using UnityEngine;

public class UpgradeNPC : MonoBehaviour
{
    [Header("대화")]
    [SerializeField] private string npcName;
    [TextArea]
    [SerializeField] private string[] dialogues;
    [SerializeField] private DialogueManager dialogueManager;


    [Header("강화")]
    [SerializeField] private int attackUpgradeCost = 100;
    [SerializeField] private int defenseUpgradeCost = 100;
    [SerializeField] private int hpUpgradeCost = 100;
    [SerializeField] private int attackUpgradeValue = 10;
    [SerializeField] private int defenseUpgradeValue = 1;
    [SerializeField] private int hpUpgradeValue = 10;
    private PlayerClass playerClass;
    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerClass = player.GetComponent<PlayerClass>();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (dialogueManager == null)
            {
                Debug.LogError("DialogueManager가 연결되지 않았습니다.");
                return;
            }

            dialogueManager.OpenDialogue(npcName, dialogues);
        }
    }

    public bool UpgradeAttack()
    {
        if (playerClass == null)
            return false;
        int cost = attackUpgradeCost * playerClass.attackUpgradeLevel;

        Debug.Log($"현재 돈: {playerClass.Money}, 공격 강화 비용: {cost}");

        if (playerClass.Money < cost)
        {
            Debug.Log("돈이 부족합니다.");
            return false;
        }

        playerClass.UseMoney(cost);
        playerClass.AddAttack(attackUpgradeValue);
        playerClass.attackUpgradeLevel++;

        Debug.Log($"공격력 강화 완료! 현재 공격력: {playerClass.Atk}");
        return true;
    }
    public bool UpgradeDefense()
    {
        if (playerClass == null)
            return false;
        int cost = defenseUpgradeCost * playerClass.defenseUpgradeLevel;

        if (playerClass.Money < cost)
        {
            Debug.Log("돈이 부족합니다.");
            return false;
        }

        playerClass.UseMoney(cost);
        playerClass.AddDefense(defenseUpgradeValue);
        playerClass.defenseUpgradeLevel++;

        Debug.Log($"방어력 강화 완료! 현재 방어력: {playerClass.Def}");
        return true;
    }
    public bool UpgradeHp()
    {
        if (playerClass == null)
            return false;
        int cost = hpUpgradeCost * playerClass.hpUpgradeLevel;

        if (playerClass.Money < cost)
        {
            Debug.Log("돈이 부족합니다.");
            return false;
        }

        playerClass.UseMoney(cost);
        playerClass.AddHp(hpUpgradeValue);
        playerClass.hpUpgradeLevel++;

        Debug.Log($"체력 강화 완료! 현재 체력: {playerClass.Hp}");
        return true;
    }
}
