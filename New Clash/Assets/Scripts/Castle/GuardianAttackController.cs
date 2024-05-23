using System;
using UnityEngine;

public class GuardianAttackController : MonoBehaviour
{
    private CastleManager _castleManager;
    private CharacterType _castleType;
    private GuardianAnimationController _guardianAnimationController;
    [HideInInspector] public GameObject ClosestTarget=null;
    private AudioSource _audioSource;
    
    private string _targetCharacterTag;

    private void Awake()
    {
        _castleManager = gameObject.transform.parent.transform.GetComponentInParent<CastleManager>();
        _guardianAnimationController = GetComponent<GuardianAnimationController>();
        _audioSource = gameObject.transform.parent.GetChild(1).transform.GetChild(0).transform
            .GetComponent<AudioSource>();
    }

    private void Start()
    {
        _castleType = _castleManager.CastleType;
        _targetCharacterTag = _castleManager.TargetCharacterTag;
    }

    private void FixedUpdate()
    {
        SearchCLoseTarget();
    }
    public void GivenDamage()
    {
        _audioSource.Play();
        ClosestTarget.GetComponent<CharacterHealthController>().TakenDamage(_castleType.GivenDamage);
        _guardianAnimationController.SetAttackAnimation(false); 
    }
    private Transform SearchCLoseTarget()
    {
        float closestDistance = Mathf.Infinity;
        GameObject[] characters = GameObject.FindGameObjectsWithTag(_targetCharacterTag);

        for (int i = 0; i < characters.Length; i++)
        {
            float distanceToTarget = Vector3.Distance(transform.position, characters[i].transform.position);
            
            if (characters[i].GetComponent<CharacterHealthController>().CurrentHealth > 0 && characters[i].activeSelf == true 
                && distanceToTarget < _castleType.TargetDisctance && distanceToTarget > 0)
            {
                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    ClosestTarget = characters[i];
                }
                _guardianAnimationController.SetAttackAnimation(true);
                
                return ClosestTarget.transform;
            }
        }
        
        return null;
    }
}
