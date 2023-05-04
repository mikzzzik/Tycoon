using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/Enemy", order = 0)]
public class EnemyParameterScriptableObject : ScriptableObject
{
    [SerializeField] private int _healthPoint;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _attackDelay;

    public int HealthPoint { get { return _healthPoint; } }
    public int AttackDamage { get { return _attackDamage; } }

    public float AttackDelay { get { return _attackDelay; } }


}
