using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealthController : MonoBehaviour
{
    public CharacterType CharacterType;
    [HideInInspector] public float CurrentHealth;
    private float _maxHealth;
    private Slider _healthSlider;
    private TextMeshProUGUI _healthText;
    public Gradient _healthSliderGradient;
    public GameObject[] Guardians;
    [SerializeField] private GameObject[] supportCastles;
    private AudioSource _destroyedAudioSource;
    

    private void Awake()
    {
        _healthSlider = gameObject.transform.parent.GetChild(2).transform.GetChild(0).GetComponent<Slider>();
        _healthText = gameObject.transform.parent.GetChild(2).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _destroyedAudioSource =
            gameObject.transform.parent.GetChild(3).transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetHealth();
    }

    private void SetHealth()
    {
        _maxHealth = CharacterType.MaxHealth;
        CurrentHealth = _maxHealth;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = CurrentHealth;
        _healthText.text = Convert.ToString(CurrentHealth);
        _healthText.color = _healthSliderGradient.Evaluate(1F);
    }

    public void TakenDamage(float damage)
    {
        if (CurrentHealth - damage <= 0)
        {
            CurrentHealth = 0;
            _healthSlider.value = 0;
        }
        else if (CurrentHealth - damage > 0)
        {
            CurrentHealth -= damage;
            _healthSlider.value = CurrentHealth;
        }
        _healthText.text = Convert.ToString(CurrentHealth);
        _healthText.color = _healthSliderGradient.Evaluate(_healthSlider.normalizedValue);
        
        CheckHealth();
    }
    private void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            for (int i = 0; i < Guardians.Length; i++)
            {
                Guardians[i].SetActive(false);
            }
            _destroyedAudioSource.Play();
            gameObject.transform.DOMoveY(gameObject.transform.position.y - 50, 0.01F);
            gameObject.transform.parent.transform.GetChild(1).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            StartCoroutine(SetActiveBytTimer(2));
        }
    }

    private IEnumerator SetActiveBytTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (gameObject.tag == "TeamRedCastle")
        {
            if (CharacterType.MaxHealth >= 3000)
            {
                PlayerPrefs.SetInt("BattleStars",3);
                for (int i = 0; i < supportCastles.Length; i++)
                {
                    supportCastles[i].GetComponent<CastleHealthController>().TakenDamage(supportCastles[i].GetComponent<CastleHealthController>().CurrentHealth);
                }
            }
            else
            {
                PlayerPrefs.SetInt("BattleStars",PlayerPrefs.GetInt("BattleStars")+1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("BotStars",PlayerPrefs.GetInt("BotStars")+1);
        }
        transform.parent.gameObject.SetActive(false);
    }
}
