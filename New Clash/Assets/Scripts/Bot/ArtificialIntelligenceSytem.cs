using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

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

    private float currentPotionAmount=3;
    private List<GameObject> battleArray;
    
    private void Start()
    {
        SetBattleArray();
        FirstInstatiate();

        // Listeleri başlat
        leftSideClosestCharacters = new List<GameObject>();
        leftSideFarCharacters = new List<GameObject>();
        rightSideClosestCharacters = new List<GameObject>();
        rightSideFarCharacters = new List<GameObject>();

        // İlk uyarıları ayarla
        SetAllerts();
    }

    private void FixedUpdate()
    {
        AutoIncreaseCurrentPotionAmount();
        SetAllerts(); 

        // Prioritize spawning if there are inactive bots or enough potion
        if (currentPotionAmount >= MinimumPotionToSpawn && !InstantiatedBots.All(bot => bot.activeSelf))
        {
            System(); 
        }
    }

    #region ArtificialIntelligence
    private float MinimumPotionToSpawn = 3f;

    private void SetBattleArray()
    {
        for (int i = 0; i <playerDataManager.player.PlayerData.MainCharacterArray.Length; i++)
        {
            battleArray.Add(Characters[i]);
        }
    }

   private void System()
{
    // Updated Threat Assessment
    bool isLeftThreatened = alertLeftSideClosest || alertLeftSideFar;
    bool isRightThreatened = alertRightSideClosest || alertRightSideFar;

    bool spawnOnLeft = false; // Default to spawn on the right
    List<GameObject> targetCharacters = new List<GameObject>();

    if (isLeftThreatened && isRightThreatened)
    {
        // Compare total threat levels on both sides (health or attack power)
        float leftThreatLevel = TotalHealthInArray(leftSideClosestCharacters) + TotalHealthInArray(leftSideFarCharacters);
        float rightThreatLevel = TotalHealthInArray(rightSideClosestCharacters) + TotalHealthInArray(rightSideFarCharacters);

        if (leftThreatLevel > rightThreatLevel)
        {
            spawnOnLeft = true;
        } // Otherwise, the default (spawnOnLeft = false) handles the right side
    }
    else if (isLeftThreatened)
    {
        spawnOnLeft = true;
    }

    // Select target characters based on the chosen side
    if (spawnOnLeft)
    {
        targetCharacters = alertLeftSideClosest ? leftSideClosestCharacters : leftSideFarCharacters;
    }
    else
    {
        targetCharacters = alertRightSideClosest ? rightSideClosestCharacters : rightSideFarCharacters;
    }

    GameObject characterToSpawn;
    Vector3 spawnPoint;

    if (targetCharacters.Count == 0) // No immediate threat
    {
        // No threat, spawn a random suitable character
        characterToSpawn = ChooseCharacterBasedOnPotion(currentPotionAmount);
        // Choose the appropriate spawn point based on the chosen side
        spawnPoint = spawnOnLeft ? GetRandomSpawnPointInLeftHalf() : GetRandomSpawnPointInRightHalf();
    }
    else // Threat detected
    {
        // Counter the threat
        characterToSpawn = ChooseCounterCharacter(targetCharacters, currentPotionAmount);
        spawnPoint = GetNearestSpawnPointToThreat(targetCharacters);
    }

    // Check if enough potion before spawning
    if (characterToSpawn.GetComponent<CharacterManager>().CharacterType.Cost <= currentPotionAmount)
    {
        BotInstantiateControl(characterToSpawn, spawnPoint);
    }
}
private Vector3 GetRandomSpawnPointInRightHalf()
{
    int randomX = Random.Range(0, maxXValue + 1); // Right half of the board
    int randomZ = Random.Range(minZValue, maxZValue);
    return new Vector3(randomX, 0, randomZ);
}
    
    private GameObject ChooseCharacterBasedOnPotion(float currentPotion)
    {
        // İksire göre uygun karakterleri filtrele
        List<GameObject> affordableCharacters = battleArray.Where(charObj => 
            charObj.GetComponent<CharacterManager>().CharacterType.Cost <= currentPotion
        ).ToList();

        // Uygun karakterler arasından rastgele birini seç
        if (affordableCharacters.Count > 0)
        {
            int randomIndex = Random.Range(0, affordableCharacters.Count);
            return affordableCharacters[randomIndex];
        }
        else
        {
            // Uygun karakter yoksa en düşük maliyetli karakteri seç
            return battleArray.OrderBy(charObj => 
                charObj.GetComponent<CharacterManager>().CharacterType.Cost
            ).First();
        }
    }
    private Vector3 GetRandomSpawnPointInLeftHalf()
    {
        int randomX = Random.Range(minXValue, 0); // Sol yarının X koordinatları (negatif değerler)
        int randomZ = Random.Range(minZValue, maxZValue + 1); // Z koordinatları aynı kalır
        return new Vector3(randomX, 0, randomZ);
    }

    // Düşman karakterlerine en yakın doğma noktasını seçer
    private Vector3 GetNearestSpawnPointToThreat(List<GameObject> threats)
    {
        GameObject closestThreat = threats.OrderBy(t => Vector3.Distance(t.transform.position, transform.position)).First();
        
        // Tehdide yakın bir nokta belirleyin (örneğin, tehdidin biraz arkasında)
        Vector3 spawnOffset = (closestThreat.transform.position - transform.position).normalized * 2f; 
        return closestThreat.transform.position + spawnOffset;
    }

    // Düşman karakterlerine karşı koyacak en uygun karakteri seçer
    private GameObject ChooseCounterCharacter(List<GameObject> enemyCharacters, float currentPotion)
    {
        GameObject bestCounter = null;
        float bestScore = -1f;

        foreach (GameObject enemy in enemyCharacters)
        {
            CharacterType enemyType = enemy.GetComponent<CharacterManager>().CharacterType;

            // Düşman karaktere karşı koyabilecek uygun karakterleri filtrele
            List<GameObject> potentialCounters = battleArray.Where(charObj =>
                    charObj.GetComponent<CharacterManager>().CharacterType.Cost <= currentPotion &&
                    charObj.GetComponent<CharacterManager>().CharacterType.CounterTo.Contains(enemyType) // CounterTo kullanımı
            ).ToList();

            if (potentialCounters.Count > 0)
            {
                // Uygun karşı karakterler arasından en yüksek puanlı olanı seç
                foreach (GameObject counter in potentialCounters)
                {
                    CharacterType counterType = counter.GetComponent<CharacterManager>().CharacterType;
                    float score = CalculateCounterScore(enemyType, counterType); // Puan hesaplama metodu

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestCounter = counter;
                    }
                }
            }
        }

        return bestCounter ?? battleArray[0]; 
    }
    
    private float CalculateCounterScore(CharacterType enemyType, CharacterType counterType)
    {
        float score = 0f;

        // Örnek puanlama mantığı:
        if (enemyType.GivenDamage > enemyType.MaxHealth)
        {
            score += counterType.MaxHealth; // Yüksek hasara karşı yüksek sağlık
        }
        else
        {
            score += counterType.GivenDamage; // Yüksek sağlığa karşı yüksek hasar
        }

        // Diğer faktörleri (menzil, hız, vb.) de hesaba katabilirsiniz

        return score;
    }

    private void BotInstantiateControl(GameObject obj, Vector3 transform)
    {
        for (int i = 0; i < InstantiatedBots.Count; i++)
        {
            CharacterType obj1Type = obj.GetComponent<CharacterManager>().CharacterType;
            CharacterType obj2Type = InstantiatedBots[i].GetComponent<CharacterManager>().CharacterType;
            if (obj1Type == obj2Type && InstantiatedBots[i].activeSelf==false)
            {
                InstantiatedBots[i].transform.position = transform;
                InstantiatedBots[i].SetActive(true);
                InstantiatedBots[i].GetComponent<CharacterMovementController>().MoveLock = false;
                InstantiatedBots[i].transform.GetChild(0).GetComponent<CharacterHealthController>().SetHealth();
                ReduceCurrentPotionAmount(InstantiatedBots[i].GetComponent<CharacterManager>().CharacterType.Cost);
                break;
            }
            if (i + 1 == InstantiatedBots.Count)
            {
                Instantiate(obj, transform, Quaternion.identity);
                ReduceCurrentPotionAmount(obj.GetComponent<CharacterManager>().CharacterType.Cost);
            }
        } 
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
        currentPotionAmount += Time.deltaTime *0.5F;
    }
    #endregion
    
}
