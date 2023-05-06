using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingContainer : MonoBehaviour
{
    [SerializeField] private ConstructionButton _constructionButton;
    
    [SerializeField] private List<BuildingScriptableObject> _buildingScriptableObject;

    private bool _isFirstInit = true;

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        if(_isFirstInit)
        {
            _isFirstInit = false;

            for (int i = 0; i < _buildingScriptableObject.Count; i++)
            {
                ConstructionButton ñonstructionButton = Instantiate(_constructionButton, transform);

                ñonstructionButton.Init(_buildingScriptableObject[i]);
            }
        }
        gameObject.SetActive(true);
    }
}
