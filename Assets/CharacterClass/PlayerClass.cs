using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerClass : Entity
{
    [SerializeField] float playerMaxHp = 100;
    [SerializeField] float playerAtk = 10;
    [SerializeField] float playerDef = 10;
    [SerializeField] public int mobCount;
    [SerializeField] public int attackUpgradeLevel = 1;
    [SerializeField] public int defenseUpgradeLevel = 1;
    [SerializeField] public int hpUpgradeLevel = 1;

    //플레이어 스탯 매 판마다 초기화
    private int level;
    private int exp;
    private int maxExp;

    // 영구 능력치
    [SerializeField] private int bonusHp;
    [SerializeField] private int bonusAtk;
    [SerializeField] private int bonusDef;

    static PlayerClass instance;
    public static PlayerClass Instance => instance;

    protected override void Awake()
    {
        base.Awake();

        entityType = EntityType.Player;

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    protected override void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Transform startPoint = GameObject.FindWithTag("StartPoint").transform;
        transform.position = startPoint.position;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
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

    public override void UpMoney(int moneyValue)
    {
        base.UpMoney(moneyValue);
    }

    public void UseMoney(int value)
    {
        money -= value;
        if (money < 0)
        {
            money = 0;
        }
    }

    public void AddAttack(int value)
    {
        bonusAtk += value;
        atk += bonusAtk;
    }
    public void AddDefense(int value)
    {
        bonusDef += value;
        def += bonusAtk;
    }
    public void AddHp(int value)
    {
        bonusHp += value;
        hp += bonusAtk;
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

        Debug.Log("레벨 업!");
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LevelUp();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("공격력 업");
            bonusAtk += 10;
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("공격력 다운");
            bonusAtk -= 10;
            SaveData();
        }
    }
}
