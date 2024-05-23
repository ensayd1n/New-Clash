using UnityEngine;

public class GuardianMovementController : MonoBehaviour
{
    private GuardianAttackController _guardianAttackController;

    private void Awake()
    {
        _guardianAttackController = gameObject.transform.GetChild(0).GetComponent<GuardianAttackController>();
    }

    private void FixedUpdate()
    {
        Rotation();
    }

    private Transform Rotation()
    {
        if (_guardianAttackController.ClosestTarget != null)
        {
            Vector3 _targetDirection = new Vector3(_guardianAttackController.ClosestTarget.transform.position.x, 0, _guardianAttackController.ClosestTarget.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            transform.rotation = Quaternion.LookRotation(_targetDirection * -1);
        }

        return transform;
    }
}
