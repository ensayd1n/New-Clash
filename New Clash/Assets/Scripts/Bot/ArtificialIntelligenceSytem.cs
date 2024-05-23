using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArtificialIntelligenceSytem : MonoBehaviour
{
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private List<GameObject> Characters;

    [SerializeField] private int minXValue, maxXValue,maxZValue;
    
    public List<GameObject> InstantiatedBots;
    
    private List<GameObject> leftSideClosestCharacters,leftSideFarCharacters;
    private List<GameObject> rightSideClosestCharacters, rightSideFarCharacters;
    
    private bool alertLeftSideClosest, alertLeftSideFar;
    private bool alertRightSideClosest, alertRightSideFar;

    private float currentPotionAmount=0;

    private GameObject currentCharacter;
    private Vector3 currentTransform;

    private void Awake()
    {
        SetBattleArray();
    }

    private void Start()
    {
        FirstInstatiate();
        StartCoroutine(SyteamByTime(3));
    }

    private void FixedUpdate()
    {
        AutoIncreaseCurrentPotionAmount();
        SetAllerts();
    }

    #region ArtificialIntelligence

    private List<GameObject> battleArray = new List<GameObject>(4);

    private void SetBattleArray()
    {
        for (int i = 0; i <playerDataManager.player.PlayerData.MainCharacterArray.Length; i++)
        {
            int randomIndex = Random.Range(0, Characters.Count);
            for (int j = 0; j < battleArray.Count; j++)
            {
                if (Characters[randomIndex] == battleArray[i])
                {
                    j--;
                }
                else
                {
                    battleArray.Add(Characters[randomIndex]);
                }
            }
            
        }
    }

    private void Sytem()
    {
        if (alertLeftSideClosest != true && alertLeftSideFar != true && alertRightSideClosest != true &&
            alertRightSideFar != true )
        {
            int randomCharacterValue = Random.Range(0, battleArray.Count);
            int randomXValue = Random.Range(minXValue, maxXValue);
            int randomZValue = Random.Range(10,maxZValue);
            currentCharacter = battleArray[randomCharacterValue];
            BotInstantiateControl(battleArray[randomCharacterValue], new Vector3(randomXValue,0,randomZValue));
        }
        else 
        {
            if (alertLeftSideClosest && currentCharacter == null)
            {
                int randomTactics = Random.Range(0, 2);
                int randomXValue = Convert.ToInt16(Random.Range(0, maxXValue));
                int randomZValue = Convert.ToInt16(Random.Range(maxZValue, 0));
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
                int randomTactics = Random.Range(0, 2);
                int randomXValue = Convert.ToInt16(Random.Range(0, maxXValue));
                int randomZValue = Convert.ToInt16(Random.Range(maxZValue, 0));
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
                int randomTactics = Random.Range(0, 2);
                int randomXValue = Convert.ToInt16(Random.Range(minXValue, 0));
                int randomZValue = Convert.ToInt16(Random.Range(maxZValue, 0));
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
                int randomTactics = Random.Range(0, 2);
                int randomXValue = Convert.ToInt16(Random.Range(minXValue, 0));
                int randomZValue = Convert.ToInt16(Random.Range(maxZValue, 0));
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
    
    #region Sytems

    private void FirstInstatiate()
    {
        for (int i = 0; i < battleArray.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject obj = Instantiate(battleArray[i]);
                InstantiatedBots.Add(obj);
            }
        }
    }
    private void BotInstantiateControl(GameObject obj, Vector3 transform)
    {
        if (obj.GetComponent<CharacterManager>().CharacterType.Cost <= currentPotionAmount && currentCharacter!=null)
        {
            GameObject obj2 = Instantiate(obj, transform, Quaternion.identity);
            obj2.transform.rotation = Quaternion.Euler(0, 180, 0);
            ReduceCurrentPotionAmount(obj.GetComponent<CharacterManager>().CharacterType.Cost);
            currentCharacter = null;
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
