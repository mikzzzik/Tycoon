using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private Building _townHall;

    [SerializeField] private Building _nowTarget;

    [SerializeField] private bool _attack;

    [SerializeField] private List<Building> _buildingTargetList = new List<Building>();

    [SerializeField] protected TextMeshPro _healthText;

    [SerializeField] private EnemyParameterScriptableObject _parameter;

    [SerializeField] private float _rotateSpeed = 0.25f;

    [SerializeField] private AttackPoint _attackPoint;

    [SerializeField] private float _remainingDistance = 0.3f;

    private int _healthPoint = 0;

    private bool _rotated;

    private Coroutine _movingCoroutine;

    int temp = 0;

    public void Init(Building townHall)
    {
        _townHall = townHall;
       _nowTarget = _townHall;
        _healthPoint = _parameter.HealthPoint;

        _healthText.text = _healthPoint.ToString();
    }


    private void Start()
    {
        _agent.enabled = true;

        Init(_townHall);
        Debug.Log(2);
        TryGetNewTarget();
    }


    private IEnumerator BeginMove()
    {
        yield return new WaitForEndOfFrame();
        _rotated = false;
        _agent.isStopped = false;
        _agent.SetDestination(_attackPoint.Position);

        yield return new WaitUntil(() => _agent.hasPath);


        Debug.Log(_agent.hasPath);
         
    }

    public bool Hit(int damage)
    {
        _healthPoint -= damage;

        if(_healthPoint<= 0)
        {
            Death();
            return true;
        }
        else
        {
            _healthText.text = _healthPoint.ToString();

            return false;
        }
    }

    private void Death()
    {
        _attackPoint.IsFree = true;
        Destroy(gameObject);
    }

    private IEnumerator Attack()
    {
        _nowTarget.Hit(_parameter.AttackDamage);
   //     Debug.Log("Attack");
        yield return new WaitForSeconds(_parameter.AttackDelay);

        if(_attack) StartCoroutine(Attack());
    }

    private void LateUpdate()
    {
        RaycastHit hit;
       
        Debug.DrawRay(transform.position + Vector3.up * 1, transform.TransformDirection(Vector3.forward) * 0.75f);
           
        if (Physics.Raycast(transform.position + Vector3.up * 1, transform.TransformDirection(Vector3.forward), out hit, 2f, ~(1 << 8)))
        {
            if (hit.distance > 0.75f || _attack || _agent.remainingDistance > _remainingDistance) return;

            _attack = true;
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;


         //   _agent.enabled = false;
         //   _navMeshObtacle.enabled = true;

            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;

            StartCoroutine(LookAt());
            StartCoroutine(Attack());
        } 
        else if (_attack)
        {
            Debug.Log("End Attack");
     
            _attack = false;

            Debug.Log(1);
            TryGetNewTarget();
        }
    }

    private void FixedUpdate()
    {
      //  Debug.Log(name + " " + _agent.remainingDistance);

        if(_agent.remainingDistance < _remainingDistance && !_rotated)
        {
            _rotated = true;

            StartCoroutine(LookAt());
        }
        if (_nowTarget != null)
        {
            if (!_nowTarget.gameObject.activeSelf)
            {
                _buildingTargetList.Remove(_nowTarget);
                _nowTarget = null;
                Debug.Log(3);
                TryGetNewTarget();
            }
        }
        if(!_attack)
            StartCoroutine(LookAt());
    }

    private IEnumerator LookAt()
    {
        Quaternion lookRotation = Quaternion.LookRotation(_nowTarget.transform.position - transform.position);

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * _rotateSpeed;

            yield return null;
        }
    }

    private void TryGetNewTarget()
    {
        Debug.Log("Try get new target");
        if (_attackPoint != null)
        {
            _attackPoint.IsFree = true;
            _attackPoint = null;
        }

        if (temp == 2) return;
        if (_buildingTargetList.Count <= 0)
        {
            _nowTarget = _townHall;

        }
        else if (_buildingTargetList.Count == 1)
        {
            _nowTarget = _buildingTargetList[0];
        }
        else
        {
            float distance = Vector3.Distance(transform.position, _buildingTargetList[0].transform.position);
            int index = 0;
            
            for (int i = 1; i < _buildingTargetList.Count; i++)
            {
                if (Vector3.Distance(transform.position, _buildingTargetList[i].transform.position) < distance)
                {
                    index = i;
                    distance = Vector3.Distance(transform.position, _buildingTargetList[i].transform.position);
                }
            }

            _nowTarget = _buildingTargetList[index];
        }

        _attackPoint = _nowTarget.GetPoint(transform.position);
        
        if(_attackPoint == null)
        {
            temp++;
            _buildingTargetList.Remove(_nowTarget);
            Debug.Log(4);
            TryGetNewTarget();

            return;
        }

        Move();
    }

    private void Move()
    {
        if(_movingCoroutine != null)
        StopCoroutine(_movingCoroutine);

        _movingCoroutine = StartCoroutine(BeginMove());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter " + other);

        Building building;
        if (other.TryGetComponent(out building))
        {
            Debug.Log(building);
            if (other.tag != "TownHall")
            {
                _buildingTargetList.Add(building);
                Debug.Log("Buidling list:" + _buildingTargetList.Count);
                if (_nowTarget == null || _nowTarget == _townHall) 
                {
                    _nowTarget = _buildingTargetList[0];
                    Debug.Log(5);
                    TryGetNewTarget();
                } 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit " + other);

        Building building;
        if (other.TryGetComponent(out building))
        {
            if (_nowTarget == building) _nowTarget = null;

            _buildingTargetList.Remove(building);
            Debug.Log(6);
            TryGetNewTarget();
        }
    }
}
