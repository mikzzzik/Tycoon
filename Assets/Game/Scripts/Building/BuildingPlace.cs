using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BuildingPlace : MonoBehaviour
{
    [SerializeField] private BuildingScriptableObject _buildingScriptableObject;
    [SerializeField] private Collider _collider;

    [SerializeField] private List<Collision> _collisionOnCollider = new List<Collision>();

    private bool _isDrag;

    public Resource[] NeedResource { get { return _buildingScriptableObject.NeedResource; } }
    private IEnumerator Start()
    {

        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        _isDrag = false;
        GridController.OnSelectGrids(transform.position, _collider.bounds.size, this);
    }

    private void OnEnable()
    {
        BuildingController.OnClearBuilding += Clear;
    }

    private void OnDisable()
    {
        BuildingController.OnClearBuilding -= Clear;
    }
    private void Update()
    {
        Debug.Log(_collisionOnCollider.Count);
        Debug.DrawRay(transform.position + new Vector3(-_collider.bounds.size.x / 2, 2, -_collider.bounds.size.z / 2), transform.TransformDirection(Vector3.right) * _collider.bounds.size.x);
        Debug.DrawRay(transform.position + new Vector3(-_collider.bounds.size.x / 2, 2, _collider.bounds.size.z / 2), transform.TransformDirection(Vector3.right) * _collider.bounds.size.x);
        Debug.DrawRay(transform.position + new Vector3(-_collider.bounds.size.x / 2, 2, -_collider.bounds.size.z / 2), transform.TransformDirection(Vector3.forward) * _collider.bounds.size.z);
        Debug.DrawRay(transform.position + new Vector3(_collider.bounds.size.x / 2, 2, -_collider.bounds.size.z / 2), transform.TransformDirection(Vector3.forward) * _collider.bounds.size.z);
        Debug.DrawRay(transform.position + new Vector3(0, 2, -_collider.bounds.size.z / 2), transform.TransformDirection(Vector3.forward) * _collider.bounds.size.z);
        Debug.DrawRay(transform.position + new Vector3(-_collider.bounds.size.x / 2, 2, 0), transform.TransformDirection(Vector3.right) * _collider.bounds.size.x);
    }

    public bool CanPlace()
    {
        Debug.Log(_collisionOnCollider.Count);
        if (_collisionOnCollider.Count > 0) return false;
        else return true;

        /*RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(-_collider.bounds.size.x / 2, 2, -_collider.bounds.size.z / 2), transform.TransformDirection(Vector3.right), out hit, _collider.bounds.size.x, ~(1 << 7)))
        {
            return false;
        }
        if (Physics.Raycast(transform.position + new Vector3(-_collider.bounds.size.x / 2, 2, _collider.bounds.size.z / 2), transform.TransformDirection(Vector3.right), out hit, _collider.bounds.size.x, ~(1 << 7)))
        {
            return false;
        }
        if (Physics.Raycast(transform.position + new Vector3(-_collider.bounds.size.x / 2, 2, -_collider.bounds.size.z / 2), transform.TransformDirection(Vector3.forward), out hit, _collider.bounds.size.z, ~(1 << 7)))
        {
            return false;
        }
        if (Physics.Raycast(transform.position + new Vector3(_collider.bounds.size.x / 2, 2, -_collider.bounds.size.z / 2), transform.TransformDirection(Vector3.forward), out hit, _collider.bounds.size.z, ~(1 << 7)))
        {
            return false;
        }
        if (Physics.Raycast(transform.position + new Vector3(0, 2, -_collider.bounds.size.z / 2), transform.TransformDirection(Vector3.forward), out hit, _collider.bounds.size.z, ~(1 << 7)))
        {
            return false;
        }
        if (Physics.Raycast(transform.position + new Vector3(-_collider.bounds.size.x / 2, 2, 0), transform.TransformDirection(Vector3.right), out hit, _collider.bounds.size.x, ~(1 << 7)))
        {
            return false;
        }
        return true;*/
    } 
    private void Clear()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collisionOnCollider.Add(collision);
        Debug.Log("Enter:" + collision.transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        _collisionOnCollider.Remove(collision);
        Debug.Log("Exit:" + collision.transform);
    }

    public void Place()
    {
        if (!CanPlace()) return;

        Building building = Instantiate(_buildingScriptableObject.Building, transform.position, transform.rotation) as Building;
        building.SetParameter(_buildingScriptableObject.Parameter);
        GridController.OnPlaceBuilding(transform.position, _collider.bounds.size);
        Destroy(gameObject);
    }


    private void OnMouseDrag()
    {
        if(!_isDrag) TouchController.OnSetMoveStatus(false);
        _isDrag = true;
        BuildingController.OnDragBuilding();
    }

    private void OnMouseUp()
    {
        _isDrag = false;
        TouchController.OnSetMoveStatus(true);
    }

    public void ChangePosition(Vector3 position)
    {
        transform.position = new Vector3(Mathf.Round(position.x), position.y, Mathf.Round(position.z));

        GridController.OnSelectGrids(transform.position, _collider.bounds.size, this);
    }
}
