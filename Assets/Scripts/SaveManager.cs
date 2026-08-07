using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public GameSaveData CurrentSaveData { get; private set; }

    private string savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(
            Application.persistentDataPath,
            "game_save.json"
        );

        Load();
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            CurrentSaveData =
                JsonUtility.FromJson<GameSaveData>(json);
        }
        else
        {
            CreateNewSaveData();
            Save();
        }

        // 데이터가 비어 있는 경우에 대한 방어 코드
        if (CurrentSaveData == null)
        {
            CreateNewSaveData();
        }

        if (CurrentSaveData.playerPermanentData == null)
        {
            CurrentSaveData.playerPermanentData =
                new PlayerPermanentData();
        }
    }

    private void CreateNewSaveData()
    {
        CurrentSaveData = new GameSaveData
        {
            playerPermanentData = new PlayerPermanentData
            {
                bonusHp = 0,
                bonusAtk = 0,
                bonusDef = 0,
                money = 0
            }
        };
    }

    public void Save()
    {
        string json =
            JsonUtility.ToJson(CurrentSaveData, true);

        File.WriteAllText(savePath, json);
    }

    public void SavePlayerPermanentData(
        PlayerPermanentData playerData)
    {
        CurrentSaveData.playerPermanentData = playerData;
        Save();
    }

    public void DeleteSaveData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        CreateNewSaveData();
        Save();
    }
}