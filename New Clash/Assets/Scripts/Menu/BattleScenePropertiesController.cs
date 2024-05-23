using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleScenePropertiesController : MonoBehaviour
{
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private Image PlayerImage;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI x2Text;
    [SerializeField] public GameObject[] uperPanelBattleStars;

    [SerializeField] private GameObject characterCardSytem;
    [SerializeField] private GameObject upperPanel;
    [SerializeField] private GameObject battleOverWinPanel;
    [SerializeField] private GameObject battleOverLosePanel;
    [SerializeField] private TextMeshProUGUI battleOverWinPanelPlayerNameText;
    [SerializeField] private TextMeshProUGUI battleOverLosePanelPlayerNameText;
    [SerializeField] private TextMeshProUGUI battleOverWinPanelIncreasePlayerScoreText;
    [SerializeField] private TextMeshProUGUI battleOverLosePanelReducePlayerScoreText;
    [SerializeField] private GameObject[] battleOverWinPanelStars;
    [SerializeField] private GameObject[] battleOverLosePanelStars;

    private float time;

    private void Start()
    {
        SetStarPlayerPrefs();
        SetUperPanelProperties();
    }

    private void FixedUpdate()
    {
        Time();
        BattleEndingController();
        BattleStarsController();
    }

    private void SetUperPanelProperties()
    {
        PlayerImage.sprite = playerDataManager.player.PlayerData.PlayerAvatarImage;
        playerName.text = playerDataManager.player.PlayerData.PlayerName;
        time = playerDataManager.player.PlayerData.BattleFirstTimeSecond;
        
    }
    private void SetStarPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("BattleStars");
        PlayerPrefs.DeleteKey("BotStars");
        PlayerPrefs.SetInt("BattleStars", 0);
        PlayerPrefs.SetInt("BotStars",0);
    }

    private void BattleStarsController()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("BattleStars");i++)
        {
            uperPanelBattleStars[i].SetActive(true);
        }  
    }

    private void BattleEndingController()
    {
        int randomScorePoint = Random.Range(20, 35);
        if (time > 0)
        {
            if (PlayerPrefs.GetInt("BattleStars") == 3)
            {
                //Kazandık
                
                SetBattleEndingWinPanelProperties(randomScorePoint);
                GameObject[] redCharacters = GameObject.FindGameObjectsWithTag("TeamRedCharacter");
                for (int i = 0; i < redCharacters.Length; i++)
                {
                    redCharacters[i].transform.parent.gameObject.SetActive(false);
                }
                characterCardSytem.SetActive(false);
                upperPanel.SetActive(false);

                
                StartCoroutine(SetActiveObject(2, battleOverWinPanel));
            }
            else if (PlayerPrefs.GetInt("BotStars") == 3)
            {
                //kaybettik
                
                SetBattleEndingLosePanelProperties(randomScorePoint);
                GameObject[] blueCharacters = GameObject.FindGameObjectsWithTag("TeamBlueCharacter");
                for (int i = 0; i < blueCharacters.Length; i++)
                {
                    blueCharacters[i].transform.parent.gameObject.SetActive(false);
                }
                characterCardSytem.SetActive(false);
                upperPanel.SetActive(false);
                
                StartCoroutine(SetActiveObject(2, battleOverLosePanel));
            }
        }
        else if (time <= 0)
        {
            if (PlayerPrefs.GetInt("BattleStars") > PlayerPrefs.GetInt("BotStars"))
            {
                //kazandık
                
                SetBattleEndingWinPanelProperties(randomScorePoint);
                GameObject[] redCharacters = GameObject.FindGameObjectsWithTag("TeamRedCharacter");
                for (int i = 0; i < redCharacters.Length; i++)
                {
                    redCharacters[i].transform.parent.gameObject.SetActive(false);
                }
                characterCardSytem.SetActive(false);
                upperPanel.SetActive(false);
                
                StartCoroutine(SetActiveObject(2, battleOverWinPanel));
            }
            else
            {
                //kaybettik
                
                SetBattleEndingLosePanelProperties(randomScorePoint);
                GameObject[] blueCharacters = GameObject.FindGameObjectsWithTag("TeamBlueCharacter");
                for (int i = 0; i < blueCharacters.Length; i++)
                {
                    blueCharacters[i].transform.parent.gameObject.SetActive(false);
                }
                characterCardSytem.SetActive(false);
                upperPanel.SetActive(false);
                StartCoroutine(SetActiveObject(2, battleOverLosePanel));
            }
        }
    }

    private void SetBattleEndingWinPanelProperties(int IncreasePlayerScore)
    {
        int randomMoneyAmount = Random.Range(100, 300);
        battleOverWinPanelPlayerNameText.text = playerDataManager.player.PlayerData.PlayerName;
        battleOverWinPanelIncreasePlayerScoreText.text = IncreasePlayerScore.ToString();
        playerDataManager.player.PlayerData.PlayerScor += IncreasePlayerScore;
        playerDataManager.player.PlayerData.BattleHistory.Add("+" + IncreasePlayerScore.ToString());
        playerDataManager.player.PlayerData.Money += randomMoneyAmount;
        playerDataManager.Save();
        UnityEngine.Time.timeScale = 0;
        int battleStarCount = PlayerPrefs.GetInt("BattleStars");

        for (int i = 0; i < battleStarCount; i++)
        {
            battleOverWinPanelStars[i].SetActive(true);
        }
    }

    private void SetBattleEndingLosePanelProperties(int reducePlayerScore)
    {
        battleOverLosePanelPlayerNameText.text = playerDataManager.player.PlayerData.PlayerName;
        battleOverLosePanelReducePlayerScoreText.text = reducePlayerScore.ToString();
        playerDataManager.player.PlayerData.PlayerScor += reducePlayerScore;
        playerDataManager.player.PlayerData.BattleHistory.Add("-" + reducePlayerScore.ToString());
        playerDataManager.Save();
        UnityEngine.Time.timeScale = 0;
        int battleStarCount = PlayerPrefs.GetInt("BotStars");

        for (int i = 0; i < battleStarCount; i++)
        {
            battleOverLosePanelStars[i].SetActive(true);
        }
    }

    private void Time()
    {
        if (time > playerDataManager.player.PlayerData.Battle2XTimeSecond)
        {
            time -= UnityEngine.Time.deltaTime;
        }
        else
        {
            x2Text.gameObject.SetActive(true);
            time -= UnityEngine.Time.deltaTime / 2;
            UnityEngine.Time.timeScale = 1.5F;
        }

        int timeMinute = Convert.ToInt16(time) / 60;
        int timeSecond = Convert.ToInt16(time) % 60;
        timeText.text = timeMinute + ":" + timeSecond;
    }

    public void QuitingTheBattle()
    {
        int randomIndex = Random.Range(20, 35);
        playerDataManager.player.PlayerData.PlayerScor -= randomIndex;
        playerDataManager.player.PlayerData.BattleHistory.Add("-" + randomIndex.ToString());
        playerDataManager.Save();
        SceneManager.LoadScene(1);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator SetActiveObject(float time, GameObject obj)
    {
        yield return new WaitForSecondsRealtime(time);
        obj.SetActive(true);
    }
}
