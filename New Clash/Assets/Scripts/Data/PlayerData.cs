using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string PlayerName = "ENES";
    public int PlayerScor = 100;
    public CharacterType[] MainCharacterArray = new CharacterType[4];
    public int ArenaLevel = 1;
    public Sprite PlayerAvatarImage = null;
    public int RightToChangeName = 1;
    public int BattleFirstTimeSecond = 180;
    public int Battle2XTimeSecond = 60;
    public List<string> BattleHistory = new List<string>(5);
    public int Money;

    public PlayerData()
    {
        
    }

    public PlayerData(string name, int playerScor, CharacterType[] mainCharacterArray,int arenaLevel,Sprite playerImage , int rightToChangeName,int battleFirstTimeSecond , int battle2XTimeSecond,List<string> battleHistory,int money)
    {
        PlayerName = name;
        PlayerScor = playerScor;
        MainCharacterArray = mainCharacterArray;
        ArenaLevel = arenaLevel;
        PlayerAvatarImage = playerImage;
        RightToChangeName = rightToChangeName;
        BattleFirstTimeSecond = battleFirstTimeSecond;
        Battle2XTimeSecond = battle2XTimeSecond;
        BattleHistory = battleHistory;
        Money = money;
    }
}
