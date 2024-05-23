using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class ShopMenuPropertiesController : MonoBehaviour
{
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] public Button[] mainCharacterArrayButtons;
    [SerializeField] private Button[] characterButtons;
    [SerializeField] private CharacterType[] characterTypes;
    [HideInInspector] public CharacterType SelectedButtonCharacterType;
    [SerializeField] private TextMeshProUGUI averagePotionText;
    [SerializeField] private TextMeshProUGUI mainArrayStatusInformationText;
    [SerializeField] private TextMeshProUGUI money;

    private void Start()
    {
        SetPlayerInformations();
        SetMainCharacterArray();
        SetCharacterButtonProperties();
        SetMainArrayProperties();
    }

    private void SetPlayerInformations()
    {
        money.text = playerDataManager.player.PlayerData.Money.ToString();
    }

    public void SetMainArrayProperties()
    {
        float totalPotion=0;
        int mainCharacterAmount = 0;
        for (int i = 0; i < playerDataManager.player.PlayerData.MainCharacterArray.Length; i++)
        {
            if (playerDataManager.player.PlayerData.MainCharacterArray[i] != null)
            {
                mainCharacterAmount++;
                totalPotion += playerDataManager.player.PlayerData.MainCharacterArray[i].Cost;
            }
        }

        totalPotion = totalPotion / mainCharacterAmount;
        averagePotionText.text = "Ort İksir: " + totalPotion;
        switch (totalPotion)
        {
            case <3:
                mainArrayStatusInformationText.text = "Durum: Düşük";
                mainArrayStatusInformationText.GetComponent<TextMeshProUGUI>().color=Color.green;
                break;
            case <5:
                mainArrayStatusInformationText.text = "Durum: Orta";
                mainArrayStatusInformationText.GetComponent<TextMeshProUGUI>().color=Color.yellow;
                break;
            case <8:
                mainArrayStatusInformationText.text = "Durum: Yüksek";
                mainArrayStatusInformationText.GetComponent<TextMeshProUGUI>().color=Color.magenta;
                break;
            case >8:
                mainArrayStatusInformationText.text = "Durum: Çok Yüksek";
                mainArrayStatusInformationText.GetComponent<TextMeshProUGUI>().color=Color.red;
                break;
        }
    }

    public void SetMainCharacterArray()
    {
        for (int i = 0; i < playerDataManager.player.PlayerData.MainCharacterArray.Length; i++)
        {
            if (playerDataManager.player.PlayerData.MainCharacterArray[i] != null)
            {
                mainCharacterArrayButtons[i].GetComponent<ButtonPropertiesController>()
                    .SetMainCharacterButtonProperties( playerDataManager.player.PlayerData.MainCharacterArray[i],playerDataManager.player.PlayerData.MainCharacterArray[i].CharacterImage,
                        playerDataManager.player.PlayerData.MainCharacterArray[i].Cost);
            }
            else
            {
                break;
            }
        }
    }

    private void SetCharacterButtonProperties()
    {
        for (int i = 0; i < characterTypes.Length; i++)
        {
            characterButtons[i].GetComponent<ButtonPropertiesController>().CharacterType = characterTypes[i];
            characterButtons[i].GetComponent<Image>().sprite = characterTypes[i].CharacterImage;
            characterButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = characterTypes[i].Cost.ToString();
        }
        for (int i = characterTypes.Length; i < characterButtons.Length; i++)
        {
            characterButtons[i].gameObject.SetActive(false);
        }
    }
}
