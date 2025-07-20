using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    public Transform objToRotate;
    public bool rotateConstantly, pauseBetweenRotations;
    [Tooltip("The formula is RotationAxis * TimeToRotate * SpeedRotation")]
    public Vector3 rotationAxis;
    public float speedRotation = 1.005f, timeToRotate = 5, timeToWait = 2;

    private bool isRotating, isAnimatedRotating;
    private float pauseTimeStamp, pauseTimer = 1;
    private float rotateTimeStamp, rotateTimer = 1;

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (!objToRotate)
            objToRotate = transform;

        pauseTimer = timeToWait;
        rotateTimer = timeToRotate;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (rotateConstantly && !pauseBetweenRotations) // we want to rotate without pausing
            RotateNow();

        if (pauseBetweenRotations) // we want to pause between rotations
        {
            if(Time.time > pauseTimeStamp + pauseTimer) // if we are done pausing
            {
                if (Time.time > rotateTimeStamp + rotateTimer) // if we done rotating
                {
                    pauseTimeStamp = Time.time;
                    isRotating = false;
                }
                else // we are not done rotating
                {
                    RotateNow();                   
                }
            }
            else // we are paused right now
            {
                rotateTimeStamp = Time.time;
            }
        }

    }


    public void SendNewRotationVector3(Vector3 _newRotation)
    {
        rotationAxis =  _newRotation;        
    }

    private void RotateNow()
    {
        isRotating = true;
        objToRotate.transform.Rotate(rotationAxis * speedRotation * Time.deltaTime);        
    }

    public void ChangeYRotation(float _newRotation)
    {
        StartCoroutine(AnimationYRotation(_newRotation));
    }

    public IEnumerator AnimationYRotation(float _newRotation)
    {
        if (!isAnimatedRotating)
        {
            int rotationDirection = 1;
            if (_newRotation < 0)
                rotationDirection = -1;

            isAnimatedRotating = true;

           for (int i = 0; i < Mathf.Abs(_newRotation); i++)
            {
                objToRotate.Rotate(0, rotationDirection, 0);
                yield return new WaitForSeconds(0.01f);
            }
            isAnimatedRotating = false;
        }
       
    }
}
