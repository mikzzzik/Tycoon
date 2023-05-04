using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _buildingPanelGameObject;
    [SerializeField] private GameObject _agreeButton;

    [SerializeField] private BuildingController _buildingController;

    public static Action OnShow;
    public static Action OnHide;

    public void Show()
    {
        if (_buildingPanelGameObject.activeSelf)
        {
            Hide();
            return;
        }

        _buildingController.CheckBuilding();
        _buildingPanelGameObject.SetActive(true);
        
        TouchController.OnSetMoveStatus(false);
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
