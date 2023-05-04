using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingController : MonoBehaviour
{
    private BuildingPlace _nowBuilding;
    [SerializeField] private Camera _camera;
    [SerializeField] private BuildingPanelUI _buildingPanelUI;
    [SerializeField] private GameObject _tilemapGameObject;
    [SerializeField] private GridController _gridController;
    [SerializeField] private ResourceController _resourceController;
    public static Action<BuildingPlace> OnChooseBuilding;
    public static Action OnDragBuilding;
    public static Action OnClearBuilding;

    private void OnEnable()
    {
        OnChooseBuilding += ChooseBuilding;
        OnDragBuilding += DragBuilding;
    }

    private void OnDisable()
    {
        OnChooseBuilding -= ChooseBuilding;
        OnDragBuilding -= DragBuilding;
    }

    public void CheckBuilding()
    {
        if (_nowBuilding != null)
        {
            OnClearBuilding?.Invoke();

            _nowBuilding = null;
        }
    }

    private void ChooseBuilding(BuildingPlace building)
    {
        _tilemapGameObject.SetActive(true);
        _nowBuilding = Instantiate(building);


        _buildingPanelUI.Hide();

        RaycastHit hit;

        Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 100, 1 << 6);

        if (hit.collider)
        {
            _nowBuilding.ChangePosition(hit.point);
        }
    }

    private void DragBuilding()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hit, 100, 1 << 6);

        Debug.DrawRay(_camera.transform.position, ray.direction * 100, Color.red, 5);

        if (hit.collider)
        {
            _nowBuilding.ChangePosition(hit.point);
        }
    }

    public void PlaceBuilding()
    {
        if (_gridController.CanPlace)
        {
            _resourceController.SpendResource(_nowBuilding.NeedResource);
            _nowBuilding.Place();
            _tilemapGameObject.SetActive(false);
        }
    }
}
