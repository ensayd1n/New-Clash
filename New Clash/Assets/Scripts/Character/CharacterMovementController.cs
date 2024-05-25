using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class CharacterMovementController : MonoBehaviour
{
    private CharacterManager _characterManager;
    [HideInInspector] public CharacterType characterType;
    private NavMeshAgent _agent;
    [HideInInspector] public bool MoveLock=false;
    private float _targetDistance;
    [HideInInspector] public GameObject ClosestTarget=null;
    [HideInInspector] public GameObject[] Targets = new GameObject[1];
    public string _targetCharacterTag,_targetCastleTag;

    [SerializeField] private bool Character, Castle;
    
    private float lastTargetSearchTime = 0;
    private float targetSearchInterval = 1f;
    

    private void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
        characterType = _characterManager.CharacterType;
        _agent = GetComponent<NavMeshAgent>(); 
        _targetDistance = characterType.TargetDisctance;
        ClosestTarget = null;
    }

    private void Start()
    {
        _targetCharacterTag = _characterManager.TargetCharacterTag;
        _targetCastleTag = _characterManager.TargetCastleTag;
    }

    private void Update()
    {
        if (Time.time - lastTargetSearchTime > targetSearchInterval || ClosestTarget == null)
        {
            SearchCLosestTarget();
            lastTargetSearchTime = Time.time;
        }

        if (ClosestTarget != null) // Hedefin geçerliliğini kontrol et
        {
            PositionAndRotation();
        }
    }
    private Transform PositionAndRotation()
    {
        if (MoveLock != true)
        {
            _agent.speed = characterType.MoveSpeed;
            if (Vector3.Distance(transform.position, ClosestTarget.transform.position) > _agent.stoppingDistance) // Hedefe ulaşılıp ulaşılmadığını kontrol et
            {
                _agent.SetDestination(ClosestTarget.transform.position);
            }
            else
            {
                // Hedefe ulaşıldığında yapılacak işlemler (örneğin saldırı, bekleme vb.)
            }
        }
        else if (MoveLock != false)
        {
            _agent.speed = 0;
        }
        return transform;
    }
    public Transform SearchCLosestTarget()
    {
        float closestDistance = Mathf.Infinity;
        GameObject[] characters = GameObject.FindGameObjectsWithTag(_characterManager.TargetCharacterTag);
        GameObject[] castles = GameObject.FindGameObjectsWithTag(_characterManager.TargetCastleTag);
        
        Targets = new GameObject[characters.Length + castles.Length];
        characters.CopyTo(Targets, 0);
        castles.CopyTo(Targets, characters.Length);
      
        for (int i = 0; i < Targets.Length; i++)
        {
            float distanceToTarget = Vector3.Distance(transform.position, Targets[i].transform.position);
                
            if (Character && Targets[i].tag == _targetCharacterTag && Targets[i].GetComponent<CharacterHealthController>().CurrentHealth>0 && Targets[i].activeSelf==true &&  Targets[i]!=gameObject)
            {
                if (distanceToTarget < _targetDistance && distanceToTarget< closestDistance && distanceToTarget>0)
                {
                    closestDistance = distanceToTarget;
                    ClosestTarget = Targets[i];
                }
            }
            else if(Castle && Targets[i].tag==_targetCastleTag)
            {
                if (closestDistance > distanceToTarget)
                {
                    closestDistance = distanceToTarget;
                    ClosestTarget = Targets[i];
                }
            }
        }
        return null;
    }

    private void MoveBlockControll()
    {
        Vector3 currentTransform = transform.GetChild(0).transform.position;
        if (currentTransform.x >= -7 && currentTransform.x <= 7 && currentTransform.z <= -35)
        {
            float distance=0;
            Vector3 currentBlockTransform=new Vector3(0,0,0);
            List<GameObject> transformControllBlocks = new List<GameObject>(GameObject.FindGameObjectsWithTag("TransformControllBlock"));
            for (int i = 0; i < transformControllBlocks.Count; i++)
            {
                float value = Math.Abs(Vector3.Distance(currentTransform, transformControllBlocks[i].transform.position));
                if (value > distance)
                {
                    distance = value;
                    currentBlockTransform = transformControllBlocks[i].transform.position;
                }
            }

            if (currentBlockTransform != null)
            {
                gameObject.transform.position = currentBlockTransform;
            }
        }
    }
}
