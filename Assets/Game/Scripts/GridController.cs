using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class GridController : MonoBehaviour
{

    [SerializeField] private int _mapWight, _mapheight;
    [SerializeField] private int _buildingZoneWight, _buildingZoneHeight;

    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private TileBase _freeTile;
    [SerializeField] private TileBase _busyTile;
    [SerializeField] private TileBase _selectTile;
    [SerializeField] private TileBase _selectBusyTile;
    [SerializeField] private TileBase _lockedTile;

    [SerializeField] private Color _freeColor;
    [SerializeField] private Color _busyColor;

    [SerializeField] private Material _buildingMaterial;
    [SerializeField] private BuildingPanelUI _buildingPanelUI;
    [SerializeField] private ResourceController _resourceController;

    private bool _canPlace;

    public static Action<Vector3,Vector3, BuildingPlace> OnSelectGrids;

    public static Action<Vector3, Vector3> OnPlaceBuilding;

    private List<Grid> _grid = new List<Grid>();

    private void OnEnable()
    {
        OnSelectGrids += SelectGrids;
        OnPlaceBuilding += PlaceBuilding;
        BuildingController.OnClearBuilding += ClearBuilding;
    }

    private void OnDisable()
    {
        OnSelectGrids -= SelectGrids;
        OnPlaceBuilding -= PlaceBuilding;

        BuildingController.OnClearBuilding -= ClearBuilding;

    }

    public bool CanPlace { get { return _canPlace; } }

    private void Awake()
    {
        for(int i = -_mapWight / 2; i <= _mapWight / 2; i++)
        {
            for(int j = -_mapheight / 2; j <= _mapheight / 2; j++)
            {
             //   Debug.Log(_tilemap.GetTile(new Vector3Int(i, j, 0)));
                if (_tilemap.GetTile(new Vector3Int(i, j, 0)) == _busyTile) break;

                if(i >= -_buildingZoneWight && i <= _buildingZoneWight && j >= -_buildingZoneHeight && j <= _buildingZoneHeight)
                    _tilemap.SetTile(new Vector3Int(i, j, 0), _freeTile);
                else
                    _tilemap.SetTile(new Vector3Int(i, j, 0), _lockedTile);
            }
        }
        _tilemap.gameObject.SetActive(false);
    }

    private void ClearBuilding()
    {
        while (_grid.Count > 0)
        {
            _tilemap.SetTile(_grid[0].Position, _grid[0].BeforeTile);
            _grid.RemoveAt(0);
        }

        _tilemap.gameObject.SetActive(false);
    }

    private void SelectGrids(Vector3 position, Vector3 bounds, BuildingPlace buildingPlace)
    {

        while (_grid.Count > 0)
        {
            _tilemap.SetTile(_grid[0].Position, _grid[0].BeforeTile);
            _grid.RemoveAt(0);
        }

        int height = Mathf.CeilToInt(bounds.z / 2);
        int wight = Mathf.CeilToInt(bounds.x / 2);

   //     Debug.Log($"Height: {height}");
  //      Debug.Log($"Wight: {wight}");

        if (buildingPlace.CanPlace() && _resourceController.EnoughtResource(buildingPlace.NeedResource))
        {
            _canPlace = true;
       //     Debug.Log("Have resource");
            for (int i = -wight; i < wight; i++)
            {
                for (int j = -height; j < height; j++)
                {
                    Vector3Int paintPosition = new Vector3Int((int)position.x + i, (int)position.z + j);

                    TileBase nowTile = _tilemap.GetTile(paintPosition);
                    TileBase newTile;
                    if (nowTile == _busyTile || nowTile == _lockedTile)
                    {
                        newTile = _selectBusyTile;
                    }
                    else
                    {
                        newTile = _selectTile;
                    }

                    if (newTile == _selectBusyTile)
                        _canPlace = false;

                    _grid.Add(new Grid(paintPosition, nowTile, newTile));

                    _tilemap.SetTile(paintPosition, newTile);
                }
            }
        }
        else
        {
            _canPlace = false;
        }

        if(_canPlace)
        {
            _buildingMaterial.color = _freeColor;
        }
        else
        {
            _buildingMaterial.color = _busyColor;
        }

        _buildingPanelUI.ChangeAgreeButtonStatus(_canPlace);
    }

    private void PlaceBuilding(Vector3 position, Vector3 bounds)
    {
        _buildingPanelUI.ChangeAgreeButtonStatus(false);
        int height = Mathf.CeilToInt(bounds.z / 2);
        int wight = Mathf.CeilToInt(bounds.x / 2);
        while (_grid.Count > 0)
        {
            _tilemap.SetTile(_grid[0].Position, _busyTile);
            _grid.RemoveAt(0);
        }

        for (int i = -wight; i < wight; i++)
        {
            for (int j = -height; j < height; j++)
            {
                Vector3Int paintPosition = new Vector3Int((int)position.x + i, (int)position.z + j);

                _tilemap.SetTile(paintPosition, _busyTile);
            }
        }
    }
    

}

[Serializable]
public class Grid
{
    public Grid(Vector3Int position, TileBase beforeTile, TileBase nowTile)
    {
        _position = position;
        _beforeTile = beforeTile;
        _nowTile = nowTile;
    }

    [SerializeField] private Vector3Int _position;
    [SerializeField] private TileBase _beforeTile;
    [SerializeField] private TileBase _nowTile;

    public Vector3Int Position { get { return _position; } }
    public TileBase BeforeTile { get { return _beforeTile; } }
    public TileBase NowTime { get { return _nowTile; } }
}
