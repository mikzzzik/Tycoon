using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _buildingPanelGameObject;
    [SerializeField] private GameObject _agreeButton;

    [SerializeField] private BuildingController _buildingController;
   
    [SerializeField] private BuildingContainer _mainBuildingContainer;

    private BuildingContainer _nowBuildingContainer;

    public void Show()
    {
        if (_buildingPanelGameObject.activeSelf)
        {
            Hide();

            return;
        }

        if(_nowBuildingContainer == null && _mainBuildingContainer != null)
        {
            _mainBuildingContainer.gameObject.SetActive(true);

            _nowBuildingContainer = _mainBuildingContainer;
        }

        _buildingController.CheckBuilding();
        _buildingPanelGameObject.SetActive(true);
        
        TouchController.OnSetMoveStatus(false);
    }

    public void ShowBuildingContent(BuildingContainer container)
    {
        if (_nowBuildingContainer != null)
        {
            _nowBuildingContainer.gameObject.SetActive(false);

        }

        _nowBuildingContainer = container;
        _nowBuildingContainer.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _buildingPanelGameObject.SetActive(false);
        TouchController.OnSetMoveStatus(true);
    }

    public void ChangeAgreeButtonStatus(bool status)
    {
        _agreeButton.SetActive(status); 
    }

}
