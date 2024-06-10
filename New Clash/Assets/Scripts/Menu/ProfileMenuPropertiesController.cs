using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ProfileMenuPropertiesController : MonoBehaviour
{
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private Image ProfileAvatarImage;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI playerArenaLevelText;
    [SerializeField] private TextMeshProUGUI playerMoneyText;
    [SerializeField] private Text changeNameAlert;
    [SerializeField] private InputField InputField;
    [SerializeField] private TextMeshProUGUI viewNameText;
    [SerializeField] private GameObject nameChangeBox;
    [SerializeField] private GameObject nameChangeAlert2Box;
    

    private void Start()
    {
        SetPlayerInformations();
        SetAudio(10);
    }
    private void SetPlayerInformations()
    {
        ProfileAvatarImage.sprite = playerDataManager.player.PlayerData.PlayerAvatarImage;
        playerNameText.text = "İsim: " + playerDataManager.player.PlayerData.PlayerName;
        playerScoreText.text = "Kupa Sayısı: " + playerDataManager.player.PlayerData.PlayerScor;
        playerArenaLevelText.text = "Arena Seviyesi: " + playerDataManager.player.PlayerData.ArenaLevel;
        changeNameAlert.text = "Kalan Hakkınız: " + playerDataManager.player.PlayerData.RightToChangeName;
        playerMoneyText.text = "Paranız: " + playerDataManager.player.PlayerData.Money;
    }

    public void SetAudio(float index)
    {
        AudioListener.volume = index;
    }

    public void ReadStringValueChanges()
    {
        viewNameText.text = InputField.text;
        
    }

    public void AcceptNewNameButtonClick()
    {
        if (viewNameText.text.Length<10 &&  playerDataManager.player.PlayerData.RightToChangeName !=0)
        {
            playerDataManager.player.PlayerData.RightToChangeName -= 1;
            playerDataManager.UpdatePlayerName(viewNameText.text.ToString());   
            playerDataManager.Save();
            nameChangeBox.SetActive(false);
            SetPlayerInformations();
        }
        else
        {
            nameChangeAlert2Box.SetActive(true);
        }
    }
}
