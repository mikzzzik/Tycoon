using UnityEngine;
using System.Collections;

 public class RemoveColliders : MonoBehaviour
{
    [SerializeField] private Transform _myObject;
    private Collider[] _childCollider;

    public void DestroyColliders()
    {
        _childCollider = _myObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in _childCollider)
        {
            DestroyImmediate(collider);
        }
    }
}
