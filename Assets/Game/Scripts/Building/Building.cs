using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using TMPro;
public class Building : MonoBehaviour
{
    [SerializeField] protected Collider _collider;
    [SerializeField] protected TextMeshPro _healthText;
    [SerializeField] protected BuildingScriptableObject _buildingScriptableObject;
    [SerializeField] private List<AttackPoint> _attackPointList;

    public BuildingScriptableObject BuildingScriptableObject { get { return _buildingScriptableObject; } }

    private Parameter _parameter;

    protected int _nowHealth;

    protected void Start()
    {
        SetParameter(_buildingScriptableObject.Parameter);
    }
    public Vector3 GetPoint(Vector3 position)
    {

        NavMeshPath path = new NavMeshPath();

        for (int i = 0; i < _attackPointList.Count; i++)
        {
            if (_attackPointList[i].IsFree)
            {
                _attackPointList[i].IsFree = false;

                NavMesh.CalculatePath(position, _attackPointList[i].Position, NavMesh.AllAreas, path);
                Debug.Log(path.status);
                Debug.Log(_attackPointList[i]);

                return _attackPointList[i].Position;

            }
        }

        return Vector3.zero;
    }

    public void Hit(int damage)
    {
        _nowHealth -= damage;

        if (_nowHealth < 0) _nowHealth = 0;

        UpdateHealthPointText();

        if (_nowHealth == 0)
            AfterLastHitAction();
    }

    public void SetParameter(Parameter parameter)
    {
        _parameter = parameter;

        _nowHealth = parameter.MaxHealthPoint;
        UpdateHealthPointText();
    }

    public void UpdateHealthPointText()
    {
        _healthText.text = _nowHealth + "/" + _parameter.MaxHealthPoint;
    }

    protected virtual void AfterLastHitAction()
    {
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class AttackPoint
{
    [SerializeField] private Transform _transformPosition;
    [SerializeField] private bool _isFree;

    public Vector3 Position {  get { return _transformPosition.position; } }
    public bool IsFree { get { return _isFree; }  set { _isFree = value; } }

}

