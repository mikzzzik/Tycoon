using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConstructionButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private List<ConstructionResource> _constructionResourceList;
    [SerializeField] private BuildingPlace _buildingPlace;

    public void Click()
    {
        BuildingController.OnChooseBuilding(_buildingPlace);
    }

    public void Init(BuildingScriptableObject buildingScriptableObject)
    {
        _name.text = buildingScriptableObject.Name;
        _icon.sprite = buildingScriptableObject.Icon;
        _buildingPlace = buildingScriptableObject.BuildingPlace;

        for (int i = 0; i < _constructionResourceList.Count; i++)
        {
            for(int j = 0; j < buildingScriptableObject.NeedResource.Length; j++)
            {
                if(_constructionResourceList[i].Type == buildingScriptableObject.NeedResource[j].Type)
                {
                    _constructionResourceList[i].TextAmount.text = buildingScriptableObject.NeedResource[j].Amount.ToString();

                    _constructionResourceList.RemoveAt(i);
                    i--;
                    break;
                }
               
            }
        }

        for (int i = 0; i < _constructionResourceList.Count; i++)
        {
            _constructionResourceList[i].TextAmount.transform.parent.gameObject.SetActive(false);
        }
    }
}


[System.Serializable]

public class ConstructionResource
{
    [SerializeField] private ResourceType _type;
    [SerializeField] private TextMeshProUGUI _textAmount;

    public ResourceType Type { get { return _type; } }
    public TextMeshProUGUI TextAmount { get { return _textAmount; } }
}
