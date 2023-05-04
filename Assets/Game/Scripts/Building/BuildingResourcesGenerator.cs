using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingResourcesGenerator : Building
{
    [Tooltip("Generate per second")]
    [SerializeField] protected List<Resource> _resourceGenerateList;
     
    private bool _isGenerate = false;

    protected void Start()
    {
        base.Start();
        BeginGenerate();
    }

    private void BeginGenerate()
    {
        _isGenerate = true;

        for (int i = 0; i < _resourceGenerateList.Count; i++)
        {
            ResourceController.OnAddResourceToGenerate(_resourceGenerateList[i]);
        }
    }

    private void StopGenerate()
    {
        if (!_isGenerate) return;

        _isGenerate = false;

        for (int i = 0; i < _resourceGenerateList.Count; i++)
        {
            ResourceController.OnRemoveResourceFromGenerate(_resourceGenerateList[i]);
        }
    }
}