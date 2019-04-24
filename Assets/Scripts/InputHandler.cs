using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Cube cubeScript;
    Vector3 startScreenPosition;
    float distance;

    bool lastRaycastHitTarget = false;
    Vector3 previousMousePosition;


    void Start()
    {
        cubeScript = GetComponent<Cube>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startScreenPosition = eventData.pointerPressRaycast.screenPosition;
        distance = eventData.pointerPressRaycast.distance;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var startWorldPosition = eventData.pointerPressRaycast.worldPosition; 

        var endScreenPosition3D = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);

        Vector3 endWorldPosition = Camera.main.ScreenToWorldPoint(endScreenPosition3D);

        Debug.DrawLine(startWorldPosition, endWorldPosition, Color.red, 50000);
        //Debug.Log($"Start OnEndDrag {startWorldPosition} , {endWorldPosition}");

        var resultVector = endWorldPosition - startWorldPosition;
        var crossVector = Vector3.Cross(eventData.pointerPressRaycast.worldNormal, resultVector); // We want to turn the cube around the axis that is nearest to this


        var axisTurningWayPair = AxisExtensions.Vector3ToAxis(crossVector, transform);

        Axis axis = axisTurningWayPair.Key;
        float turningWay = Mathf.Sign(axisTurningWayPair.Value);

        //Debug.Log($"Chosen Axis: {axis}");

        float positionValue = AxisExtensions.GetValueAtAxis(axis, eventData.pointerPressRaycast.gameObject.transform.localPosition);

        cubeScript.RotateSide(axis, positionValue, turningWay*90);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            
            lastRaycastHitTarget = Physics.Raycast(ray);
        }
        else if (Input.GetMouseButton(0) && !lastRaycastHitTarget)
        {
            // Rotating the whole cube only when the click didn't hit anything.
            cubeScript.TurnCube(Input.mousePosition - previousMousePosition);
        }

        previousMousePosition = Input.mousePosition;
    }

}
