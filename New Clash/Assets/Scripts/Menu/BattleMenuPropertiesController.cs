using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleMenuPropertiesController : MonoBehaviour
{
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private Sprite[] arenaSpoilers;
    [SerializeField] private Image arenaSpoilerImage;
    [SerializeField] private TextMeshProUGUI AreaInformationText;
    public GameObject MessageBox;
    [SerializeField] private TextMeshProUGUI[] battleHistoryTexts;
    [SerializeField] private Image[] battleHistorTextBackGround;

    [SerializeField] private GameObject LoadSceneCanva;

    
    private void Awake()
    {
        SetBattleHistoryView();
    }
    private void Start()
    {
        SetPlayerInformations();
    }
    private void SetPlayerInformations()
    {
        AreaInformationText.text = "Arena Seviyeniz: " + playerDataManager.player.PlayerData.ArenaLevel.ToString();
        arenaSpoilerImage.sprite = arenaSpoilers[playerDataManager.player.PlayerData.ArenaLevel - 1];
    }
    
    
    public void StartBattleButton()
    {
        int index = 0;
        for (int i = 0; i < playerDataManager.player.PlayerData.MainCharacterArray.Length; i++)
        {
            if (playerDataManager.player.PlayerData.MainCharacterArray[i] != null)
            {
                index++;
            }  
        }

        if (index == playerDataManager.player.PlayerData.MainCharacterArray.Length)
        {
            gameObject.SetActive(false);
            LoadSceneCanva.SetActive(true);
            LoadSceneCanva.GetComponent<LoadSceneController>().LoadScene(playerDataManager.player.PlayerData.ArenaLevel+1);LoadSceneCanva.SetActive(true);
        }
        else
        {
            MessageBox.SetActive(true);
        }
        
    }
    
    private void SetBattleHistoryView()
    {
        int index = 0;
        for (int i = playerDataManager.player.PlayerData.BattleHistory.Count-battleHistoryTexts.Length; i < playerDataManager.player.PlayerData.BattleHistory.Count; i++)
        {
            if (playerDataManager.player.PlayerData.BattleHistory[i] is not null)
            {
                if (playerDataManager.player.PlayerData.BattleHistory[i].Substring(0, 1) == "+")
                {
                    battleHistoryTexts[index].text = playerDataManager.player.PlayerData.BattleHistory[i];
                    battleHistorTextBackGround[index].color=Color.green;
                }
                else if (playerDataManager.player.PlayerData.BattleHistory[i].Substring(0, 1) == "-")
                {
                    battleHistoryTexts[index].text = playerDataManager.player.PlayerData.BattleHistory[i];
                    battleHistorTextBackGround[index].color=Color.red;
                }
                else
                {
                    battleHistoryTexts[index].text = "--";
                    battleHistorTextBackGround[index].color=Color.grey;
                }
            }
            else
            {
                battleHistoryTexts[index].text = "--";
                battleHistorTextBackGround[index].color=Color.grey;
            }
            index++;
        }
    }
}