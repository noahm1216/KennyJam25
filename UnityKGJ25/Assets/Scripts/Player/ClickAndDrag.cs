using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ClickAndDrag : MonoBehaviour
{
    [Header("REFERENCES\n___________")]
    public Transform playerObj;
    public Camera playerCamera;
    public LayerMask myLayerMask;
    public LineRenderer pullbackLineRen;

    [Space][Space]
    [Header("CONTROLS\n___________")]
    public bool lockDragWhileMoving;
    public bool rotateDirectionTossed = true;

    [Space][Space]
    [Header("POWER\n___________")]
    public int dragForceBase = 10;
    public bool limitYVelocity;
    [Range(0, 10)]
    public float timeUntilDeaccelerate = 2.5f;    
    [Range(0,1)]
    public float deaccelerationRate = 0.99f;
    public float stopVelocity = 0.5f;

    private float timeSinceRelease;
    private bool canDragObject = true;
    private bool draggingFromObject;
    private bool boatIsDocked;
    private bool weWon;
    private Rigidbody rbody;

    public UnityEvent onLaunchEvent;

    [Space]
    [Space]
    [Header("POLISH\n___________")]
    public Transform playerShip;
    public Transform sinkingShip;


    private void Update() // Update is called once per frame
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

        if (Input.GetKeyUp(KeyCode.Mouse0))
            ReleaseMouseHold();

        if (Input.GetKeyUp(KeyCode.Escape))
        { Scene currentScene = SceneManager.GetActiveScene(); SceneManager.LoadScene(currentScene.name); }
    }


    private void FixedUpdate()
    {
        if (rbody)
        {
            //print($"Velocity {rbody.velocity }");
            //print($"Megnitude {(int)rbody.velocity.magnitude }");
            //print($"SqMagnitude {rbody.velocity.sqrMagnitude }");

            if (rbody.velocity.magnitude < stopVelocity && !boatIsDocked && Time.time > timeSinceRelease + timeUntilDeaccelerate)
            {
                rbody.velocity = Vector3.zero;
                if (lockDragWhileMoving) canDragObject = true;

                if (!weWon)
                { print("WE DID NOT MAKE IT... restarting in 3"); StartCoroutine(DelaySceneReload(3.5f)); }
            }
            else
            {
                if (Time.time > timeSinceRelease + timeUntilDeaccelerate)
                    rbody.velocity *= deaccelerationRate;
            }
        }
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
            if (hit.point.z > playerObj.transform.position.z) hit.point = new Vector3(hit.point.x, playerObj.position.y, playerObj.position.z);
            return hit.point;
        }
        //if (pullbackLineRen) pullbackLineRen.enabled = false;
        return Vector3.zero;
    }

    public void ReleaseMouseHold()  //print("release mouse left click");
    {
        onLaunchEvent?.Invoke();

        if (lockDragWhileMoving) canDragObject = false;
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
        {
            if (limitYVelocity) _direction.y = 0;
            rbody.AddForce(_direction * dragForceBase - rbody.velocity, ForceMode.VelocityChange);
            DockBoat(false);
        }
        else
            Debug.LogWarning($"Missing Rigidbody on: {playerObj.name}");

        if (rotateDirectionTossed)
            playerObj.transform.rotation = Quaternion.LookRotation(_direction);

        //SimpleCameraEffects.Instance.PlayShakeEffect(3f,9f);
    }

    public void DockBoat(bool _dockBoat)
    {
        boatIsDocked = _dockBoat;
    }

    public void ChangeWin(bool _didWin)
    {
        weWon = _didWin;
    }

    public IEnumerator DelaySceneReload(float _timeToWait)
    {
        playerShip.gameObject.SetActive(false);
        sinkingShip.gameObject.SetActive(true);
        yield return new WaitForSeconds(_timeToWait);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
