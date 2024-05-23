using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator _animator = new Animator();

    private void Awake()
    {
        _animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    public void SetAttackAnimation(bool isActive)
    {
        _animator.SetBool("Attack",isActive);
    }

    public void SetDieAnimation(bool isActive)
    {
        _animator.SetBool("Die",isActive);
    }
}
