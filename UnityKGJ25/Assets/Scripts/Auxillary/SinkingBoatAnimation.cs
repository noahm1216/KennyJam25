using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingBoatAnimation : MonoBehaviour
{
    public float sinkSpeed = 1;
    private Vector3 startPosition;
    private float enableTimeStamp, enableTimer = 5;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        gameObject.SetActive(false);
        
    }

    private void OnEnable()
    {
        transform.localPosition = startPosition;
        enableTimeStamp = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > enableTimeStamp + enableTimer)
            gameObject.SetActive(false);

        transform.Translate(Vector3.up * -sinkSpeed * Time.deltaTime, Space.World);
    }
}
