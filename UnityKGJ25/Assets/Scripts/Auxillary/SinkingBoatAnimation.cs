using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingBoatAnimation : MonoBehaviour
{

    public float sinkSpeed = 1;
    private Vector3 startPosition;
    public bool resetOnEnable = true, disableOnTimer = true;
    private float enableTimeStamp, enableTimer = 5;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        gameObject.SetActive(false);        
    }

    private void OnEnable()
    {
        if (resetOnEnable) transform.localPosition = startPosition;
        enableTimeStamp = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > enableTimeStamp + enableTimer && disableOnTimer)
            gameObject.SetActive(false);

        if (Time.time < enableTimeStamp + enableTimer)
            transform.Translate(Vector3.up * -sinkSpeed * Time.deltaTime, Space.World);
    }
}
