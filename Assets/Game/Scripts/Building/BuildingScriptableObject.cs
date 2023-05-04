using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Building/Building", order = 0)]
public class BuildingScriptableObject : ScriptableObject
{
    [SerializeField] private Resource[] _needResource;
    [SerializeField] private Building _building;
    [SerializeField] private Parameter _parameter;

    public Building Building { get { return _building; } } 
    public Resource[] NeedResource { get { return _needResource; } }
    public Parameter Parameter { get { return _parameter; } }
}

[System.Serializable]
public class Parameter
{
    [SerializeField] private int _maxHealthPoint;
    [SerializeField] private int _attackOrder;

    public int MaxHealthPoint { get { return _maxHealthPoint; } }
    public int AttackOrder { get { return _attackOrder; } }
}