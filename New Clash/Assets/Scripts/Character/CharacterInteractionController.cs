using UnityEngine;

public class CharacterInteractionController : MonoBehaviour
{
    private CharacterAttackController _attackController;
    private CharacterAnimationController _animationController;
    private CharacterMovementController _characterMovementController;
    private CharacterManager _characterManager;
    private CharacterHealthController _characterHealthController;
    
    private bool _interactionToTargetLock = true;
    

    private void Awake()
    {
        _characterMovementController =  gameObject.transform.parent.GetComponentInParent<CharacterMovementController>();
        _attackController = gameObject.transform.parent.GetChild(0).GetComponent<CharacterAttackController>();
        _animationController = gameObject.transform.parent.GetComponent<CharacterAnimationController>();
        _characterManager = gameObject.transform.parent.GetComponentInParent<CharacterManager>();
        _characterHealthController = gameObject.transform.parent.transform.GetChild(0).transform
            .GetComponent<CharacterHealthController>();
    }
    
    private void FixedUpdate()
    {
        ProximityLimitation();

    }

   private void ProximityLimitation()
    {
        if (_characterMovementController.ClosestTarget != null && _characterHealthController.CurrentHealth > 0)
        {
            float distanceToTarget = Vector3.Distance(gameObject.transform.parent.transform.position, _characterMovementController.ClosestTarget.transform.position);
            bool isTargetInContact = _attackController.InteractionTarget != null; // Hedefle temas halinde mi kontrolü

            if (_characterMovementController.ClosestTarget.tag == _characterManager.TargetCharacterTag)
            {
                if (distanceToTarget <= _characterManager.CharacterType.TargetDisctance 
                    && _interactionToTargetLock != true
                    && _characterMovementController.ClosestTarget.GetComponent<CharacterHealthController>()
                        .CurrentHealth > 0
                    && isTargetInContact) // Hedefle temas halinde mi kontrolü eklendi
                {
                    _attackController.InteractionTarget = _characterMovementController.ClosestTarget;
                    gameObject.GetComponentInParent<CharacterMovementController>().MoveLock = true;
                    _animationController.SetAttackAnimation(true);
                }
                else
                {
                    _attackController.InteractionTarget = null;
                    gameObject.GetComponentInParent<CharacterMovementController>().MoveLock = false;
                    _animationController.SetAttackAnimation(false);
                }
            }
            else if (_characterMovementController.ClosestTarget.tag == _characterManager.TargetCastleTag)
            {
                if (distanceToTarget <= _characterManager.CharacterType.TargetDisctance 
                    && _interactionToTargetLock != true
                    && isTargetInContact) // Hedefle temas halinde mi kontrolü eklendi
                {
                    _attackController.InteractionTarget = _characterMovementController.ClosestTarget;
                    gameObject.GetComponentInParent<CharacterMovementController>().MoveLock = true;
                    _animationController.SetAttackAnimation(true);
                }
                else
                {
                    _attackController.InteractionTarget = null;
                    gameObject.GetComponentInParent<CharacterMovementController>().MoveLock = false;
                    _animationController.SetAttackAnimation(false);
                }
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(_characterManager.TargetCharacterTag) || other.gameObject.CompareTag(_characterManager.TargetCastleTag))
        {
            float distanceToTarget = Vector3.Distance(gameObject.transform.parent.transform.position, other.gameObject.transform.position);
            if (distanceToTarget <= _characterManager.CharacterType.TargetDisctance && other.gameObject == _characterMovementController.ClosestTarget)
            {
                _interactionToTargetLock = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(_characterManager.TargetCharacterTag) || other.gameObject.CompareTag(_characterManager.TargetCastleTag))
        {
            _interactionToTargetLock = true; 
        }
    }

}
