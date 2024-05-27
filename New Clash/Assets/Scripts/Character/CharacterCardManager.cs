using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardManager : MonoBehaviour
{
   public CharacterType CharacterType;
   [SerializeField] private TextMeshProUGUI Name;
   [SerializeField] private Image CharacterImage;
   [SerializeField] private TextMeshProUGUI HealthText;
   [SerializeField] private TextMeshProUGUI AttackText;
   [SerializeField] private TextMeshProUGUI CostText;
   

   public void SetCardCharactersProperties()
   {
      Name.text = CharacterType.Name;
      CharacterImage.GetComponent<Image>().sprite = CharacterType.CharacterImage;
      HealthText.text =  Convert.ToString(CharacterType.MaxHealth);
      AttackText.text =  Convert.ToString(CharacterType.GivenDamage);
      CostText.text = Convert.ToString(CharacterType.Cost);
   }
}
