using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    [Header("REFERENCES\n___________")]
    public Transform playerObj;
    public Camera playerCamera;
    public LayerMask myLayerMask;
    public LineRenderer pullbackLineRen;

    [Space][Space]
    [Header("CONTROLS\n___________")]
    public bool LockDragWhileMoving;

    [Space][Space]
    [Header("POWER\n___________")]
    public int dragForceBase = 10;
    [Range(0, 10)]
    public float timeUntilDeaccelerate = 2.5f;    
    [Range(0,1)]
    public float deaccelerationRate = 0.99f;
    public float stopVelocity = 0.5f;

    private float timeSinceRelease;
    private bool canDragObject = true;
    private bool draggingFromObject;
    private Rigidbody rbody;
    
        
    void Update() // Update is called once per frame
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canDragObject)
            RaycastMouseIsOverMe();

        if (Input.GetKey(KeyCode.Mouse0) && draggingFromObject)
        {
            if (pullbackLineRen)
                pullbackLineRen.SetPosition(0, DraggingMousePosition());
            else
                pullbackLineRen.SetPosition(0, playerObj.position);

            pullbackLineRen.SetPosition(1, playerObj.position);
        }

        if (rbody)
        {
            if (rbody.velocity.sqrMagnitude < stopVelocity)
            {
                rbody.velocity = Vector3.zero;
                if (LockDragWhileMoving) canDragObject = true;
            }
            else
            {
                if(Time.time > timeSinceRelease + timeUntilDeaccelerate)
                rbody.velocity *= deaccelerationRate;
            }

        }       

        if (Input.GetKeyUp(KeyCode.Mouse0))
            ReleaseMouseHold();
    }

    public bool RaycastMouseIsOverMe() // check if mouse is on our player object layer
    {
        if (!playerObj)
            playerObj = transform;

        if (!playerCamera)
            playerCamera = Camera.main;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, myLayerMask))
        {
            Debug.Log("Did Hit");
            draggingFromObject = true;
            if (pullbackLineRen) pullbackLineRen.enabled = true;
            return true;
        }
        draggingFromObject = false;
        return false;
    }

    public Vector3 DraggingMousePosition()  //print("holding mouse left click");
    {
        if (!playerCamera)
            playerCamera = Camera.main;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }
        if (pullbackLineRen) pullbackLineRen.enabled = false;
        return Vector3.zero;
    }

    public void ReleaseMouseHold()  //print("release mouse left click");
    {      
        if (LockDragWhileMoving) canDragObject = false;
        timeSinceRelease = Time.time;

        if (draggingFromObject)
        {
            draggingFromObject = false;
            if (pullbackLineRen)
                LaunchObject(pullbackLineRen.GetPosition(1) - pullbackLineRen.GetPosition(0));
            else
                Debug.LogWarning($"Missing LineRenderer for: {playerObj.name}");
        }
        if (pullbackLineRen) pullbackLineRen.enabled = false;
    }

    public void LaunchObject(Vector3 _direction)
    {
        if (!rbody)
            playerObj.TryGetComponent(out rbody);

        if (rbody)
            rbody.AddForce(_direction * dragForceBase - rbody.velocity, ForceMode.VelocityChange);
        else
            Debug.LogWarning($"Missing Rigidbody on: {playerObj.name}");        
    }


}
