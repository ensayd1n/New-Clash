using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthController : MonoBehaviour
{
    private CharacterType _characterType;
    private CharacterMovementController _characterMovementController;
    private CharacterAnimationController _characterAnimationController;
    private CharacterManager _characterManager;

    [HideInInspector] public float CurrentHealth;
    private float _maxHealth;

    public GameObject _canvas;
    private Slider _healthSlider;

    private AudioSource _audioSource;

    private float _keeperHealthBarByTimer = 0;

    private void Awake()
    {
        _characterMovementController = gameObject.transform.parent.GetComponent<CharacterMovementController>();
        _characterType = gameObject.transform.parent.GetComponent<CharacterManager>().CharacterType;
        _characterAnimationController = gameObject.transform.parent.GetComponent<CharacterAnimationController>();
        _characterManager = gameObject.transform.parent.GetComponent<CharacterManager>();
        _healthSlider = gameObject.transform.parent.gameObject.transform.GetChild(2).transform.GetChild(0).GetComponent<Slider>();
        _audioSource = gameObject.transform.parent.transform.GetChild(3).transform.GetChild(1).transform
            .GetComponent<AudioSource>();
        _canvas.SetActive(false);
    }

    private void Start()
    {
        SetHealth();
    }

    private void FixedUpdate()
    {
        //SetActiveCanvas();
    }

    public void SetHealth()
    {
        _maxHealth = _characterType.MaxHealth;
        CurrentHealth = _maxHealth;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = CurrentHealth;
    }

    public void TakenDamage(float damage)
    {
        if (CurrentHealth - damage <= 0)
        {
            CurrentHealth = 0;
            _healthSlider.value = 0;
            _canvas.SetActive(true);
        }
        else if (CurrentHealth - damage > 0)
        {
            CurrentHealth -= damage;
            _healthSlider.value = CurrentHealth;
            _canvas.SetActive(true);
        }
        _keeperHealthBarByTimer = 4;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            _audioSource.Play();
            _characterAnimationController.SetAttackAnimation(false);
            _characterAnimationController.SetDieAnimation(true);
            _characterMovementController.MoveLock = true;
        }
    }

    private void SetActiveCanvas()
    {
        _keeperHealthBarByTimer = Mathf.Clamp(_keeperHealthBarByTimer, 0, 4);
        if (_keeperHealthBarByTimer <= 0)
        {
            _canvas.SetActive(false);
        }
        else if (_keeperHealthBarByTimer > 0)
        {
            _keeperHealthBarByTimer -= Time.deltaTime;
            _canvas.SetActive(true);
        }
    }

    public void Die()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
