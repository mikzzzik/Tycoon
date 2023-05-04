using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildingButton : MonoBehaviour
{
    [SerializeField] private BuildingPlace _buildingPlace;
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        if (_button != null)
        {
            _button = GetComponent<Button>();
        }
    }

    private void Awake()
    {
        if (!_buildingPlace) return;


        _button.onClick.AddListener(() =>
        {
            BuildingController.OnChooseBuilding(_buildingPlace);
        });
    }
}
