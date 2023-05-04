using UnityEngine;

[CreateAssetMenu(fileName = "Tower_tier_", menuName = "Building/Tower", order = 1)]

public class TowerScriptableObject : BuildingScriptableObject
{
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _attackDelay;
    public int AttackDamage { get { return _attackDamage; } }

    public float AttackDelay { get { return _attackDelay; } }
}
