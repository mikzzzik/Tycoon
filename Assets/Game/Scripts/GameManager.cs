using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResourceController _resourceController;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("FirstTime"))
        {
            _resourceController.Init();
        }
        else
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            _resourceController.FirstInit();
        }
    }
}
