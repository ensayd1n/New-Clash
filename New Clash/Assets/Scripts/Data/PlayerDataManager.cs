using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour
{
    public Player player;

    private void Awake()
    {
        FirstData();
        Load();
    }

    public void UpdatePlayerName(string newPlayerName)
    {
        player.PlayerData.PlayerName = newPlayerName;
    }

    #region Save&Load
    [ContextMenu("Save")]
    public void Save()
    {
        string json = JsonUtility.ToJson(player, true);
        File.WriteAllText(Application.persistentDataPath + "/PlayerData.json",json);
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/PlayerData.json");
            player = JsonUtility.FromJson<Player>(json);
        }
    }

    public void FirstData()
    {
        if (!File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            player.PlayerData.PlayerAvatarImage=Resources.Load<Sprite>("Sprites/OP1");
            player.PlayerData.PlayerName = "AAAA";
            player.PlayerData.PlayerScor = 100;
            player.PlayerData.ArenaLevel = 1;
            player.PlayerData.RightToChangeName = 1;
            player.PlayerData.BattleFirstTimeSecond = 180;
            player.PlayerData.Battle2XTimeSecond = 60;
            for (int i = 0; i < player.PlayerData.MainCharacterArray.Length; i++)
            {
                player.PlayerData.MainCharacterArray[i] = null;
            }
            for (int i = 0; i < 5; i++)
            {
                player.PlayerData.BattleHistory.Add("AAAA");
            }
            player.PlayerData.Money = 0;
            Save();
        }
    }
    #endregion
}

[System.Serializable]
public class Player
{
    public PlayerData PlayerData = new PlayerData();
}
