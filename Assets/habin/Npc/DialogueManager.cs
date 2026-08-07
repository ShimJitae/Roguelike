using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button ChoiceButton1;
    [SerializeField] private Button ChoiceButton2;
    [SerializeField] private Button ChoiceButton3;

    [SerializeField] private UpgradeNPC upgradeNPC;

    private PlayerClass playerClass;
    private string[] currentDialogues;
    private int currentIndex;
    private bool isResultShowing;
    private void Start()
    {
        playerClass = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerClass>();

    }
    private void Update()
    {
        if (isResultShowing && Input.GetKeyDown(KeyCode.Z) && Input.GetMouseButtonDown(0))
        {
            CloseDialogue();
            isResultShowing = false;
        }
    }
    public void OpenDialogue(string npcName, string[] dialogues)
    {
        currentDialogues = dialogues;
        currentIndex = 0;

        dialoguePanel.SetActive(true);
        choicePanel.SetActive(true);

        npcNameText.text = npcName;
        dialogueText.text = currentDialogues[currentIndex];
    }
    public void NextDialogue()
    {
        currentIndex++;

        if (currentIndex >= currentDialogues.Length)
        {
            ShowChoice();
            return;
        }
    }
    private void ShowChoice()
    {
        choicePanel.SetActive(true);
        dialogueText.text = "강화하시겠습니까?";
    }
    public void SelectAtk()
    {
        bool success = upgradeNPC.UpgradeAttack();
        if (success)
        {
            dialogueText.text = "공격력 강화에 성공했습니다.";
        }
        else
        {
            dialogueText.text = "마석이 부족합니다.";
        }
        choicePanel.SetActive(false);
        isResultShowing = true;
    }
    public void SelectDef()
    {
        bool success = upgradeNPC.UpgradeDefense();

        if (success)
        {
            dialogueText.text = "방어력 강화에 성공했습니다.";
        }
        else
        {
            dialogueText.text = "마석이 부족합니다.";
        }

        choicePanel.SetActive(false);
        isResultShowing = true;

    }
    public void SelectNo()
    {
        Debug.Log("거절 선택");

        choicePanel.SetActive(false);
        CloseDialogue();

    }
    public void SelectHp()
    {
        Debug.Log("체력 버튼 클릭됨");
        bool success = upgradeNPC.UpgradeHp();

        if (success)
        {
            dialogueText.text = "체력 강화에 성공했습니다.";
        }
        else
        {
            dialogueText.text = "마석이 부족합니다.";
        }

        choicePanel.SetActive(false);
        isResultShowing = true;

    }
    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
    }
   
}


