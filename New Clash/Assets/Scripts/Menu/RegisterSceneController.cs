using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterSceneController : MonoBehaviour
{
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private InputField InputField;
    [SerializeField] private Image viewPlayerAvatar;
    private Sprite currentSelectedSprite;
    private string currentSelectedPlayerName;
    [SerializeField] private TextMeshProUGUI viewNameText;
    private bool selectedImage=false;
    [SerializeField] private GameObject LoadSceneCanva;
    
    public void SetPlayerAvatar(GameObject avatar)
    {
        currentSelectedSprite = avatar.GetComponent<Image>().sprite;
        viewPlayerAvatar.sprite = avatar.GetComponent<Image>().sprite;
        selectedImage = true;
    }

    public void SetPlayerName()
    {
        currentSelectedPlayerName = InputField.text;
        viewNameText.text = currentSelectedPlayerName;
    }

    public void RegisterButtonClick()
    {
        if (InputField.text.Length <10 && selectedImage==true)
        {
            playerDataManager.player.PlayerData.PlayerName = currentSelectedPlayerName;
            playerDataManager.player.PlayerData.PlayerAvatarImage = currentSelectedSprite;
            playerDataManager.Save();
            LoadSceneCanva.SetActive(true);
            LoadSceneCanva.GetComponent<LoadSceneController>().LoadScene(1);
        }
    }
    
}
