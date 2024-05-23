using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPropertiesController : MonoBehaviour
{
    [SerializeField] private ShopMenuPropertiesController shopMenuPropertiesController;
    [SerializeField] private ScrollRect scrollRect;
    [HideInInspector] public CharacterType CharacterType;
    [HideInInspector] public int MainCharacterButtonIndex;
    [SerializeField] private PlayerDataManager playerDataManager;
    
    public void SetMainCharacterButtonProperties(CharacterType type,Sprite img,float cost)
    {
        CharacterType = type;
        gameObject.GetComponent<Image>().sprite = img;
        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cost.ToString();
    }
    public void ClickMainCharacterButton()
    {
        if (shopMenuPropertiesController.SelectedButtonCharacterType!=null && SetMainCharacterButtonControll(shopMenuPropertiesController.SelectedButtonCharacterType))
        {
            playerDataManager.player.PlayerData.MainCharacterArray[MainCharacterButtonIndex] =
                shopMenuPropertiesController.SelectedButtonCharacterType;
            playerDataManager.Save();
            SetMainCharacterButtonProperties(
                shopMenuPropertiesController.SelectedButtonCharacterType,
                gameObject.GetComponent<ButtonPropertiesController>().shopMenuPropertiesController.SelectedButtonCharacterType.CharacterImage,
                gameObject.GetComponent<ButtonPropertiesController>().shopMenuPropertiesController.SelectedButtonCharacterType.Cost);
            shopMenuPropertiesController.SetMainArrayProperties();
            shopMenuPropertiesController.SelectedButtonCharacterType = null;
        }
    }

    private bool SetMainCharacterButtonControll(CharacterType type)
    {
        for (int i = 0; i < shopMenuPropertiesController.mainCharacterArrayButtons.Length; i++)
        {
            if (shopMenuPropertiesController.mainCharacterArrayButtons[i].GetComponent<ButtonPropertiesController>()
                    .CharacterType == type)
            {
                return false;
            }
        }
        return true;
    }
    
    public void ClickCharacterButton()
    {
        shopMenuPropertiesController.SelectedButtonCharacterType = CharacterType;
        scrollRect.verticalNormalizedPosition = 1;
    }
}
