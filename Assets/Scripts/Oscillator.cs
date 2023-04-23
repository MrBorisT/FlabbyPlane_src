using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] float speed = 1.19f;
    [SerializeField] float xOffset = 0.3f;
    Vector3 pointA;
    Vector3 pointB;

    void Start()
    {
        pointA = new Vector3(-xOffset, 0, 0);
        pointB = new Vector3(xOffset, 0, 0);
    }
    void Update()
    {
        float time = Mathf.PingPong(Time.time * speed, 1);
        transform.localPosition = Vector3.Lerp(pointA, pointB, time);
    }
}
