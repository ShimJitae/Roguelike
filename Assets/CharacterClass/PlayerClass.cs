using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerClass : Entity
{
    [SerializeField] float playerMaxHp = 100;
    [SerializeField] float playerAtk = 10;
    [SerializeField] float playerDef = 10;
    [SerializeField] public int mobCount;

    //플레이어 스탯 매 판마다 초기화
    private int level;
    private int exp;
    private int maxExp;

    // 영구 능력치
    private int bonusHp;
    private int bonusAtk;
    private int bonusDef;
    private int money;

    protected override void Awake()
    {
        base.Awake();

        entityType = EntityType.Player;
        LoadPermanentData();
        InitializeRunData();
        InitializeStats();
    }

    private void LoadPermanentData()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError(
                "SaveManager가 존재하지 않습니다."
            );

            return;
        }

        PlayerPermanentData data =
            SaveManager.Instance
                .CurrentSaveData
                .playerPermanentData;

        bonusHp = data.bonusHp;
        bonusAtk = data.bonusAtk;
        bonusDef = data.bonusDef;
        money = data.money;
    }

    private void InitializeRunData()
    {
        level = 1;
        exp = 0;
        maxExp = 10;
    }

    private void InitializeStats()
    {
        maxHp = playerMaxHp + bonusHp;
        hp = maxHp;

        atk = playerAtk + bonusAtk;
        def = playerDef + bonusDef;
    }

    //던전 클리어시 초기화
    public void StatReset()
    {
        InitializeRunData();
        InitializeStats();
    }


    public void GetExp(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        exp += amount;

        // 초과된 경험치도 계산
        while (exp >= maxExp)
        {
            exp -= maxExp;
            LevelUp();
        }
    }


    // 레벨업시 능력치 상승
    public void LevelUp()
    {
        level++;
        maxExp += (int)(level * 1.2f);
        maxHp += (int)(level * 1.2);
        atk += (int)(level * 1.2);
        def += (int)(level * 1.2);
    }

    // 영구 능력치 데이터 저장
    // 저장 시기
    /*  상점에서 업그레이드 시
     *  던전 클리어 시
     */
    public void SaveData()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError(
                "SaveManager가 존재하지 않습니다."
            );

            return;
        }

        PlayerPermanentData data = new PlayerPermanentData
        {
            bonusHp = bonusHp,
            bonusAtk = bonusAtk,
            bonusDef = bonusDef,
            money = money
        };

        SaveManager.Instance.SavePlayerPermanentData(data);

        Debug.Log("플레이어 영구 데이터 저장 완료");
    }

}
