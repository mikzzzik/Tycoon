using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private List<Resource> _resourceList;
    [SerializeField] private List<ResourceUI> _resourceUIList;

    [SerializeField] private List<Resource> _generateResourceList = new List<Resource>();

    public static Action<Resource> OnAddResourceToGenerate;
    public static Action<Resource> OnRemoveResourceFromGenerate;

    private void AddResourceToGerenerate(Resource resource)
    {
        Debug.Log($"Add {resource}");

        for (int i  = 0 ; i < _generateResourceList.Count; i++)
        {
            if(_generateResourceList[i].Type == resource.Type)
            {
                _generateResourceList[i].Amount += resource.Amount;

                return;
            }
        }

        _generateResourceList.Add(resource);
    }

    private void RemoveResourceFromGenerate(Resource resource)
    {
        Debug.Log($"Remove {resource}");
        for (int i = 0; i < _generateResourceList.Count; i++)
        {
            if (_generateResourceList[i].Type == resource.Type)
            {
                _generateResourceList[i].Amount -= resource.Amount;

                if (_generateResourceList[i].Amount < 0) _generateResourceList[i].Amount = 0;

                return;
            }
        }

    }

    private IEnumerator GenerateResource()
    {
        yield return new WaitForSeconds(1f);
        
        for(int i =0; i < _generateResourceList.Count;i++)
        {
            ChangeResourceAmount(_generateResourceList[i].Type, _generateResourceList[i].Amount);
        }

        StartCoroutine(GenerateResource());

        UpdateUI();
    }

    public void Init()
    {
        for (int i = 0; i < _resourceList.Count; i++)
        {
            ChangeResourceAmount(_resourceList[i].Type,PlayerPrefs.GetInt(_resourceList[i].Type.ToString()));
        }

        UpdateUI();

        StartCoroutine(GenerateResource());
    }    

    public void FirstInit()
    {
        for(int i =0;i < _resourceList.Count; i++)
        {
            ChangeResourceAmount(_resourceList[i].Type, 1000);
        }
        UpdateUI();
    }

    public void ChangeResourceAmount(ResourceType type, int amount)
    {
        for (int i = 0; i < _resourceList.Count; i++)
        {
            if (type == _resourceList[i].Type)
            {
                _resourceList[i].Amount += amount;
            }
        }
    }

    public void UpdateUI()
    {
        for(int i = 0; i < _resourceUIList.Count; i++)
        {
            _resourceUIList[i].UpdateUI(_resourceList[i].Amount);
        }
    }

    public bool EnoughtResource(Resource[] resourceArray)
    {
        for (int i = 0; i < resourceArray.Length; i++)
        {
            if (!EnoughtResource(resourceArray[i]))
                return false;
        }

        return true;
    }

    public bool EnoughtResource(Resource resource)
    {
        for (int i = 0; i < _resourceList.Count; i++)
        {
            if (resource.Type == _resourceList[i].Type)
            {
                if (resource.Amount > _resourceList[i].Amount)
                {
                    return false;
                }
                else
                {
                    break;
                }
            }
        }

        return true;
    }

    public void SpendResource(Resource[] resourceArray)
    {
        for (int i = 0; i < resourceArray.Length; i++)
        {
            SpendResource(resourceArray[i]);
        }

        UpdateUI();
    }

    public void SpendResource(Resource resource)
    {
        for (int i = 0; i < _resourceList.Count; i++)
        {
            if (resource.Type == _resourceList[i].Type)
            {
                _resourceList[i].Amount -= resource.Amount;
            }
        }
    }

    private void OnEnable()
    {
        OnAddResourceToGenerate += AddResourceToGerenerate;
        OnRemoveResourceFromGenerate += RemoveResourceFromGenerate;

     for (int i = 0;i < _resourceUIList.Count;i++)
        {
            _resourceList.Add(new Resource(_resourceUIList[i].Type, 0));
        }
    }

    private void OnDisable()
    {
        OnAddResourceToGenerate -= AddResourceToGerenerate;
        OnRemoveResourceFromGenerate -= RemoveResourceFromGenerate;

        for (int i = 0;i < _resourceList.Count; i++)
        {
            PlayerPrefs.SetInt(_resourceList[i].Type.ToString(), _resourceList[i].Amount);
        }
    }
}

public enum ResourceType
{
    Wood,
    Stone,
    Money
}
