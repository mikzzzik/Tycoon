using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TouchController : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] private float[] _zoomBounds = new float[] { 10f, 25f };
    [SerializeField] float _zoomSpeed = 2f;
    [SerializeField] float _moveSpeed = 0.2f;

    public static Action<bool> OnSetMoveStatus;

    private bool _canMove = true;
    private Vector3 _beginPosition;

    private void OnEnable()
    {
        OnSetMoveStatus += SetMoveStatus;
    }

    private void OnDisable()
    {
        OnSetMoveStatus -= SetMoveStatus;
    }

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    private void SetMoveStatus(bool status)
    {
        _canMove = status;
    }

    private void Update()
    {
        if (!_canMove) return;

#if UNITY_EDITOR
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        ZoomCamera(scroll, _zoomSpeed);

#elif UNITY_ANDROID
        Debug.Log("Android");

        if (Input.touchCount >= 2)
        {
            var pos1 = Input.GetTouch(0).position;
            var pos2 = Input.GetTouch(1).position;
            var pos1b = (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            var zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);
            Debug.Log(zoom);

            ZoomCamera(zoom - 1, _zoomSpeed);
            return;
        }
#endif


        if (Input.GetMouseButton(0))
            {
                if (_beginPosition != Vector3.zero)
                {
                    MoveCamera(Input.mousePosition);
                }

                _beginPosition = Input.mousePosition;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                _beginPosition = Vector3.zero;
            }
        
    }

    private void MoveCamera(Vector3 newPosition)
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position,_camera.transform.position + (new Vector3(newPosition.x,0, newPosition.y) - new Vector3(_beginPosition.x, 0, _beginPosition.y)).normalized, _moveSpeed);
    }

    void ZoomCamera(float offset, float speed)
    {
  //      Debug.Log($"{_camera.transform.position.y }     {offset * speed}");
        if (offset == 0 || offset < 0 && _camera.transform.position.y >= _zoomBounds[1] || offset > 0 && _camera.transform.position.y <= _zoomBounds[0]) return;

       // Debug.Log($"{_camera.transform.position.y }     {offset * speed}");
        Vector3 position = _camera.transform.position + _camera.transform.forward * offset * speed;

        _camera.transform.position = position;
    }

#if UNITY_IOS
    [SerializeField] Camera _camera;
    public bool Rotate;
    public Plane Plane;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    private void Update()
    {

        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll
        if (Input.touchCount >= 1)
        {
            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                _camera.transform.Translate(Delta1, Space.World);
        }

        //Pinch
        if (Input.touchCount >= 2)
        {
            var pos1  = PlanePosition(Input.GetTouch(0).position);
            var pos2  = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            var zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);
            Debug.Log(zoom);
            //edge case
            if (zoom == 0 || zoom > 10)
                return;

            //Move cam amount the mid ray
            _camera.transform.position = Vector3.LerpUnclamped(pos1, _camera.transform.position, 1 / zoom);

            if (Rotate && pos2b != pos2)
                _camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
        }

    }

    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = _camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = _camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = _camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
#endif
}
