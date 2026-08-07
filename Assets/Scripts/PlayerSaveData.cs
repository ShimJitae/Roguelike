using System;

[Serializable]
public class PlayerPermanentData
{
    public int bonusHp;
    public int bonusAtk;
    public int bonusDef;
    public int money;
}

[Serializable]
public class GameSaveData
{
    public PlayerPermanentData playerPermanentData;
}