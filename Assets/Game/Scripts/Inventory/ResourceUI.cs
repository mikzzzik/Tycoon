using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private ResourceType _type;

    public ResourceType Type { get { return _type; } }

    public bool IsResource(ResourceType type)
    {
        if(type == _type)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateUI(int amount)
    {
        if (amount > 0)
            _text.text = string.Format("{0:#,#}", amount);
        else
            _text.text = "0";
    }
}

[System.Serializable]
public class Resource
{
    [SerializeField] private ResourceType _type;
    [SerializeField] private int _amount;

    public Resource(ResourceType type, int amount)
    {
        _type = type;
        _amount = amount;
    }

    public ResourceType Type { 
        get { return _type; } 
    }

    public int Amount {
        get { return _amount; }
        set { _amount = value; }
    }
}