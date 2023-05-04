using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{

    [SerializeField] private List<Enemy> _enemyInTriggerList = new List<Enemy>();

    [SerializeField] private Enemy _target;

    private int _damage;
    private float _attackDelay;

    private void Awake()
    {
        _damage = (_buildingScriptableObject as TowerScriptableObject).AttackDamage;
        _attackDelay = (_buildingScriptableObject as TowerScriptableObject).AttackDelay;
    }

    private IEnumerator Attack()
    {
        if (_target == null && _enemyInTriggerList.Count > 0)
        {
            for (int i = 0; i < _enemyInTriggerList.Count; i++)
            {
                if (_enemyInTriggerList[i] != null)
                {
                    _target = _enemyInTriggerList[i];
                }
                else
                {
                    _enemyInTriggerList.RemoveAt(i);
                    i--;
                }
            }
        }
        else if (_enemyInTriggerList.Count <= 0 )
        {
            yield break;
        }

        yield return new WaitForSeconds(_attackDelay);

        if(_target != null)
        {
            if (_target.Hit(_damage))
            {
                _enemyInTriggerList.Remove(_target);
            };
        }

        StartCoroutine(Attack());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy;

            if (other.TryGetComponent<Enemy>(out enemy))
            {
                _enemyInTriggerList.Add(enemy);
               
                if(_target == null)
                    StartCoroutine(Attack());
            }
        }
    }

    private void TrySetNewTarget()
    {
        _target = null;

        if (_enemyInTriggerList.Count >= 0)
        {
            _target = _enemyInTriggerList[0];
        }

    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Enemy")
        {
            Enemy enemy;

            if (other.TryGetComponent<Enemy>(out enemy))
            {
                Debug.Log(_target == enemy);

                if (_target == enemy)
                {
                    TrySetNewTarget();
                }

                _enemyInTriggerList.Remove(enemy);
            }
        }
    }
}
