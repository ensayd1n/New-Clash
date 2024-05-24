using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArtificialIntelligenceSytem : MonoBehaviour
{
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private List<GameObject> Characters;

    private int minXValue=-13, maxXValue=13,minZValue=12,maxZValue=35;
    
    public List<GameObject> InstantiatedBots;
    
    private List<GameObject> leftSideClosestCharacters,leftSideFarCharacters;
    private List<GameObject> rightSideClosestCharacters, rightSideFarCharacters;
    
    private bool alertLeftSideClosest=false, alertLeftSideFar=false;
    private bool alertRightSideClosest=false, alertRightSideFar=false;

    private float currentPotionAmount=0;
    
    private void Start()
    {
        SetBattleArray();
        FirstInstatiate();
        StartCoroutine(SyteamByTime(3));
    }

    private void FixedUpdate()
    {
        AutoIncreaseCurrentPotionAmount();
        SetAllerts();
    }

    #region ArtificialIntelligence

    public List<GameObject> battleArray;
    private GameObject selectedInstantiateCharacter=null;

    private void SetBattleArray()
    {
        for (int i = 0; i <playerDataManager.player.PlayerData.MainCharacterArray.Length; i++)
        {
            battleArray.Add(Characters[i]);
        }
    }

    private void Sytem()
    {
        //Her hangi bir tehdit yok ise bu kısım çalıştırılacak
        if (alertLeftSideClosest != true && alertLeftSideFar != true && alertRightSideClosest != true &&
            alertRightSideFar != true && selectedInstantiateCharacter==null)
        {
            Debug.Log(selectedInstantiateCharacter);
            int randomCharacterValue = Random.Range(0, battleArray.Count);
            int randomXValue = Random.Range(minXValue, maxXValue);
            int randomZValue = Random.Range(10,maxZValue);
            selectedInstantiateCharacter = battleArray[randomCharacterValue];
            BotInstantiateControl(battleArray[randomCharacterValue], new Vector3(randomXValue,0,randomZValue));
        }
        else
        {
            Debug.Log("tehdit aldı");
            //tehdit var ise hangi tarafta olduğuna göre çalıştırılacak method lar
            if (alertLeftSideClosest)
            {
                //sol yakın kısımda bir tehdit var ve burası çalışacak
                int randomTactics = Random.Range(0, 2);
                //karakterin hangi konumlarda oluşturulacağı belirleniyor
                int randomXValue = Convert.ToInt16(Random.Range(0, maxXValue));
                int randomZValue = Convert.ToInt16(Random.Range(0, maxZValue));
                // bir taktik seçimi yapılarak ona göre oluşturulma yapılıyor cana bağlı yada saldırı gücüne bağlı bir taktik seçimi
                switch (randomTactics)
                {
                    case 0: BotInstantiateControl(SetSuitableCharacterForHealth(SetLeftSideClosestCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                    case 1: BotInstantiateControl(SetSuitableCharacterForAttackPower(SetLeftSideClosestCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                    default: BotInstantiateControl(SetSuitableCharacterForHealth(SetLeftSideClosestCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                }
            }
            else if (alertLeftSideFar)
            {
                //sol uzak tarafta bir tehdit var ise burası tetiklenicek
                int randomTactics = Random.Range(0, 2);
                //karakterin hangi konumlarda oluşturulacağı belirleniyor
                int randomXValue = Convert.ToInt16(Random.Range(0, maxXValue));
                int randomZValue = Convert.ToInt16(Random.Range(0, maxZValue));
                // bir taktik seçimi yapılarak ona göre oluşturulma yapılıyor cana bağlı yada saldırı gücüne bağlı bir taktik seçimi
                switch (randomTactics)
                {
                    case 0: BotInstantiateControl(SetSuitableCharacterForHealth(SetLeftSideFarCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                    case 1: BotInstantiateControl(SetSuitableCharacterForAttackPower(SetLeftSideFarCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                    default: BotInstantiateControl(SetSuitableCharacterForHealth(SetLeftSideFarCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                }
            }
            else if (alertRightSideClosest)
            {
                //sağ yakın tarafta bir tehdir var ise burası tetiklenicek
                int randomTactics = Random.Range(0, 2);
                //karakterin hangi konumlarda oluşturulacağı belirleniyor
                int randomXValue = Convert.ToInt16(Random.Range(minXValue, 0));
                int randomZValue = Convert.ToInt16(Random.Range(0, maxZValue));
                // bir taktik seçimi yapılarak ona göre oluşturulma yapılıyor cana bağlı yada saldırı gücüne bağlı bir taktik seçimi
                switch (randomTactics)
                {
                    case 0: BotInstantiateControl(SetSuitableCharacterForHealth(SetRightSideClosestCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                    case 1: BotInstantiateControl(SetSuitableCharacterForAttackPower(SetRightSideClosestCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                    default: BotInstantiateControl(SetSuitableCharacterForHealth(SetRightSideClosestCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                }
            }
            else if (alertRightSideFar)
            {
                //sağ uzak tarafta bir tehdir var ise burası tetiklenicek
                int randomTactics = Random.Range(0, 2);
                //karakterin hangi konumlarda oluşturulacağı belirleniyor
                int randomXValue = Convert.ToInt16(Random.Range(minXValue, 0));
                int randomZValue = Convert.ToInt16(Random.Range(0, maxZValue));
                // bir taktik seçimi yapılarak ona göre oluşturulma yapılıyor cana bağlı yada saldırı gücüne bağlı bir taktik seçimi
                switch (randomTactics)
                {
                    case 0: BotInstantiateControl(SetSuitableCharacterForHealth(SetRightSideFarCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                    case 1: BotInstantiateControl(SetSuitableCharacterForAttackPower(SetRightSideFarCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                    default: BotInstantiateControl(SetSuitableCharacterForHealth(SetRightSideFarCharactersArray()),new Vector3(randomXValue,0,randomZValue));
                        break;
                }
            }
        }
    }
    private void BotInstantiateControl(GameObject obj, Vector3 transform)
    {
        for (int i = 0; i < InstantiatedBots.Count; i++)
        {
            if (InstantiatedBots[i] == obj)
            {
                obj.transform.position = transform;
                obj.SetActive(true);
                ReduceCurrentPotionAmount(obj.GetComponent<CharacterManager>().CharacterType.Cost);
                selectedInstantiateCharacter = null;
                break;
            }
            if (i + 1 == InstantiatedBots.Count)
            {
                Instantiate(obj, transform, Quaternion.identity);
                ReduceCurrentPotionAmount(obj.GetComponent<CharacterManager>().CharacterType.Cost);
                selectedInstantiateCharacter = null;
            }
        } 
    }
    
    private IEnumerator SyteamByTime(float time)
    {
        yield return new WaitForSeconds(time);
        Sytem();
        StartCoroutine(SyteamByTime(time));
    }

    #endregion

    #region Suitability

    private GameObject SetSuitableCharacterForHealth(List<GameObject> Array)
    {
        GameObject obj=null;
        float difference = Mathf.Infinity;
        for (int i = 0; i < battleArray.Count; i++)
        {
            if (Math.Abs(TotalHealthInArray(Array) -
                         battleArray[i].GetComponent<CharacterManager>().CharacterType.MaxHealth) <= difference)
            {
                obj = battleArray[i];
            }
        }
        return obj;
    }
    private GameObject SetSuitableCharacterForAttackPower(List<GameObject> Array)
    {
        GameObject obj=null;
        float difference = Mathf.Infinity;
        for (int i = 0; i < battleArray.Count; i++)
        {
            if (Math.Abs(TotalAttackPowerInArray(Array) -
                         battleArray[i].GetComponent<CharacterManager>().CharacterType.GivenDamage) <= difference)
            {
                obj = battleArray[i];
            }
        }
        return obj;
    }

    #endregion

    #region Arrays
    private List<GameObject> SetBotsArray()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("TeamBlueCharacter");
        
        if (objs.Length == 0)
        {
            return new List<GameObject>(); 
        }

        List<GameObject> InstantiatedBots = new List<GameObject>();

        for (int i = 0; i < objs.Length; i++)
        {
            InstantiatedBots.Add(objs[i].transform.parent.gameObject);
        }

        return InstantiatedBots;
    }
    private List<GameObject> SetLeftSideClosestCharactersArray()
    {
        if (SetBotsArray() == null || SetBotsArray().Count == 0)
        {
            return new List<GameObject>(); 
        }

        List<GameObject> leftSideClosestCharacters = new List<GameObject>();

        for (int i = 0; i < SetBotsArray().Count; i++)
        {
            if (SetBotsArray()[i].transform.position.z > 0 && SetBotsArray()[i].transform.position.x >= 10)
            {
                leftSideClosestCharacters.Add(SetBotsArray()[i]);
            }
        }

        return leftSideClosestCharacters;
    }
    private List<GameObject> SetLeftSideFarCharactersArray()
    {
        if (SetBotsArray() == null || SetBotsArray().Count == 0)
        {
            return new List<GameObject>();
        }

        List<GameObject> leftSideFarCharacters = new List<GameObject>();

        for (int i = 0; i < SetBotsArray().Count; i++)
        {
            if (SetBotsArray()[i].transform.position.z < 0 && SetBotsArray()[i].transform.position.x < 10)
            {
                leftSideFarCharacters.Add(SetBotsArray()[i]);
            }
        }

        return leftSideFarCharacters;
    }
    private List<GameObject> SetRightSideClosestCharactersArray()
    {
        if (SetBotsArray() == null || SetBotsArray().Count == 0)
        {
            return new List<GameObject>();
        }

        List<GameObject> rightSideClosestCharacters = new List<GameObject>();

        for (int i = 0; i < SetBotsArray().Count; i++)
        {
            if (SetBotsArray()[i].transform.position.z <= 0 && SetBotsArray()[i].transform.position.x >= 10)
            {
                rightSideClosestCharacters.Add(SetBotsArray()[i]);
            }
        }

        return rightSideClosestCharacters;
    }
    private List<GameObject> SetRightSideFarCharactersArray()
    {
        if (SetBotsArray() == null || SetBotsArray().Count == 0)
        {
            return new List<GameObject>();
        }

        List<GameObject> rightSideFarCharacters = new List<GameObject>();

        for (int i = 0; i < SetBotsArray().Count; i++)
        {
            if (SetBotsArray()[i].transform.position.z < 0 && SetBotsArray()[i].transform.position.x < 10)
            {
                rightSideFarCharacters.Add(SetBotsArray()[i]);
            }
        }

        return rightSideFarCharacters;
    }
    #endregion

    #region ArrayInformations
    private float TotalHealthInArray(List<GameObject> array)
    {
        float value=1;
        for (int i = 0; i < array.Count; i++)
        {
            value += array[i].GetComponent<CharacterManager>().CharacterType.MaxHealth;
        }
        return value;
    }
    private float TotalAttackPowerInArray(List<GameObject> array)
    {
        float value=1;
        for (int i = 0; i < array.Count; i++)
        {
            value += array[i].GetComponent<CharacterManager>().CharacterType.MaxHealth;
        }
        return value;
    }

    #endregion
    
    #region OtherGameMethod

    private void FirstInstatiate()
    {
        for (int i = 0; i < battleArray.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject obj = Instantiate(battleArray[i]);
                InstantiatedBots.Add(obj);
                obj.SetActive(false);
            }
        }
    }
    private void SetAllerts()
    {
        if (SetLeftSideClosestCharactersArray().Count + SetLeftSideFarCharactersArray().Count >
            SetRightSideClosestCharactersArray().Count + SetRightSideFarCharactersArray().Count)
        {
            if (SetLeftSideClosestCharactersArray().Count >= SetLeftSideFarCharactersArray().Count)
            {
                alertLeftSideClosest = true;
            }
            else alertLeftSideFar = true;
        }
        else if(SetRightSideClosestCharactersArray().Count + SetRightSideFarCharactersArray().Count >= SetLeftSideClosestCharactersArray().Count + SetLeftSideFarCharactersArray().Count)
        {
            if (SetRightSideClosestCharactersArray().Count >= SetRightSideFarCharactersArray().Count)
            {
                alertRightSideClosest = true;
            }
            else alertRightSideFar = true;
        }
        else
        {
            alertLeftSideClosest = false;
            alertLeftSideFar = false;
            alertRightSideClosest = false;
            alertRightSideFar = false;
        }
    }
    #endregion
    
    #region Potion
    private void ReduceCurrentPotionAmount(float amount)
    {
        if (currentPotionAmount - amount >= 0)
        {
            currentPotionAmount -= amount;
        }
        else currentPotionAmount = 0;
    }

    private void AutoIncreaseCurrentPotionAmount()
    {
        currentPotionAmount = Mathf.Clamp(currentPotionAmount, 0, 10);
        currentPotionAmount += Time.deltaTime;
    }
    #endregion
    
}
