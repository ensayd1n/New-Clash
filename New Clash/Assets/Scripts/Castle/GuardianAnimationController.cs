using UnityEngine;

public class GuardianAnimationController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator =GetComponent<Animator>();
    }

    public void SetAttackAnimation(bool isActive)
    {
        _animator.SetBool("Attack",isActive);
    }
}
