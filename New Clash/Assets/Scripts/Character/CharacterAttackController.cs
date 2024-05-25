using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{
    private CharacterType _characterType;
    private CharacterHealthController _characterHealthController;
    private CharacterManager _characterManager;
    [HideInInspector] public GameObject InteractionTarget = null;
    private AudioSource _audioSource;

    private void Awake()
    {
        _characterType = gameObject.transform.GetComponentInParent<CharacterManager>().CharacterType;
        _audioSource = gameObject.transform.parent.transform.GetChild(3).transform.GetChild(0).transform
            .GetComponent<AudioSource>();
        _characterManager =gameObject.transform.GetComponentInParent<CharacterManager>();
    }

    public void GivenDamage()
    {
        if (InteractionTarget != null)
        {
            if (InteractionTarget.tag == _characterManager.TargetCharacterTag)
            {
                InteractionTarget.GetComponent<CharacterHealthController>()
                    .TakenDamage(_characterType.GivenDamage);
            }
            else if (InteractionTarget.tag == _characterManager.TargetCastleTag)
            {
                InteractionTarget.GetComponent<CastleHealthController>()
                    .TakenDamage(_characterType.GivenDamage);
            }
            _audioSource.Play();
        }
    }
    
}
