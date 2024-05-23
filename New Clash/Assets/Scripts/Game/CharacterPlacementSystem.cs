using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPlacementSystem : MonoBehaviour
{
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private CharacterType[] characterTypes;
    public List<GameObject> instaniateCharacters;
    [SerializeField] private GameObject[] placementLimitViewBlocks;

    private void Start()
    {
        SetCardCharacterTypes();
        FirstInstantiate();
    }
    private void FixedUpdate()
    {
        AutoIncreasePotion();
        CardAccessibilityController();
    }
    private void Update()
    {
        PlacementCharacter();
    }

    #region InstantiateCharacterControll

    private void FirstInstantiate()
    {
        for (int i = 0; i < playerDataManager.player.PlayerData.MainCharacterArray.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject obj = Instantiate(playerDataManager.player.PlayerData.MainCharacterArray[i].ThisGameObject);
                obj.transform.position = new Vector3(0, 0, 0);
                obj.SetActive(false);
                instaniateCharacters.Add(obj);
            }
        }
    }

    private GameObject FindCharacter(CharacterType type)
    {
        for (int j = 0; j < instaniateCharacters.Count; j++)
        {
            if (instaniateCharacters[j].activeSelf == false &&
                instaniateCharacters[j].GetComponent<CharacterManager>().CharacterType == type &&
                instaniateCharacters[j].transform.GetChild(0).tag=="TeamBlueCharacter")
            {
                return instaniateCharacters[j];
            }
        }   
        return null;
    }

    private GameObject InstantiateCharacter(CharacterType inputType , Transform transform)
    {
        if (FindCharacter(inputType)!=null)
        {
            GameObject obj = FindCharacter(inputType);
            obj.transform.position = new Vector3(transform.position.x,0,transform.position.z);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(inputType.ThisGameObject, new Vector3(transform.position.x,0,transform.position.z), Quaternion.identity);
            instaniateCharacters.Add(obj);
            return obj;
        }
    }
    #endregion

    #region Card
    public Button[] CardSlots;

    private void SetCardCharacterTypes()
    {
        for (int i = playerDataManager.player.PlayerData.MainCharacterArray.Length; i < 0; i--)
        {
            CardSlots[i].GetComponent<CharacterCardManager>().CharacterType =
                playerDataManager.player.PlayerData.MainCharacterArray[i];
        }
    }
    #endregion

    #region PlacementSytem

    [SerializeField] private GameObject MouseIndicator,CellIndicator;
    [SerializeField] private InputManager InputManager;
    [SerializeField] private Grid Grid;
    [SerializeField] private GameObject GridVisualization;
    [SerializeField] private AudioSource InstatiateCharacterAudio;
    private GameObject _selectedSlot;
    private GameObject _selectedObject;
    private void PlacementCharacter()
    {
        if (_selectedObject == null && _selectedSlot == null)
            return;
        
        Vector3 mousePosition = InputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = Grid.WorldToCell(mousePosition);
        MouseIndicator.transform.position = Grid.CellToWorld(gridPosition);
        CellIndicator.transform.position = Grid.CellToWorld(gridPosition);
    }
    public void StartPlacement(GameObject slot)
    {
        _selectedSlot = slot;
        _selectedObject =
            Instantiate(_selectedSlot.GetComponent<CharacterCardManager>().CharacterType.ThisHologramParentGameObject);
        MouseIndicator = _selectedObject;
        GridVisualization.SetActive(true);
        CellIndicator.SetActive(true);
        InputManager.OnClicked += PlaceStrucature;
        InputManager.OnExit += StopPlacement;
        for (int i = 0; i < placementLimitViewBlocks.Length; i++)
        {
            placementLimitViewBlocks[i].SetActive(true);
        }
    }

    private void PlaceStrucature()
    {
        if (InputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = InputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = Grid.WorldToCell(mousePosition);
        _selectedObject.transform.position = Grid.CellToWorld(gridPosition);
        StopPlacement();
    }
    private void StopPlacement()
    {
        GameObject obj = InstantiateCharacter(_selectedSlot.GetComponent<CharacterCardManager>().CharacterType, MouseIndicator.transform);
        ReducePotion(_selectedSlot.GetComponent<CharacterCardManager>().CharacterType.Cost);
        Destroy(_selectedObject);
        InstatiateCharacterAudio.Play();
        MouseIndicator = null;
        GridVisualization.SetActive(false);
        CellIndicator.SetActive(false);
        _selectedSlot.GetComponent<Button>().interactable = false;
        StartCoroutine(SetActiveRetryButton(5, _selectedSlot));
        _selectedObject = null;
        _selectedSlot = null;
        InputManager.OnClicked -= PlaceStrucature;
        InputManager.OnExit -= StopPlacement;
        for (int i = 0; i < placementLimitViewBlocks.Length; i++)
        {
            placementLimitViewBlocks[i].SetActive(false);
        }
    }

    private IEnumerator SetActiveRetryButton(float time,GameObject obj)
    {
        yield return new WaitForSecondsRealtime(time);
        obj.GetComponent<Button>().interactable = true;
    }
    
    
    #endregion

    #region CardAccessibilitController
    private void CardAccessibilityController()
    {
        for (int i = 0; i < CardSlots.Length; i++)
        {
            if (_currentPotion < CardSlots[i].GetComponent<CharacterCardManager>().CharacterType.Cost)
            {
                CardSlots[i].interactable = false;
            }
            else
            {
                CardSlots[i].interactable = true;
            }
        }
    }

    #endregion
    
    #region Potion
    
    [SerializeField] private float potionRegenerationSpeed=1F;
    public Slider PotionSlider;
    public TextMeshProUGUI PotionText;
    private float _maxPotiion = 10;
    private float _currentPotion = 3;

    private void ManuelIncreasePotion(float amount)
    {
        _currentPotion = Mathf.Clamp(_currentPotion, 0, 10);
        _currentPotion += amount;
    }
    private void AutoIncreasePotion()
    {
        _currentPotion = Mathf.Clamp(_currentPotion, 0, 10);
        _currentPotion += Time.deltaTime * potionRegenerationSpeed ;
        SetSlider();
    }

    public void ReducePotion(float amount)
    {
        if (_currentPotion - amount >= 0)
        {
            _currentPotion -= amount;
        }

        SetSlider();
    }

    private void SetSlider()
    {
        PotionText.text = _currentPotion.ToString("F0");
        PotionSlider.maxValue = _maxPotiion;
        PotionSlider.value = _currentPotion;
    }
    #endregion
}
