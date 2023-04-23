using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [SerializeField] float scrollingSpeed;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform spawnPoint;
    [SerializeField] bool infinite;

    public Transform EndPoint { set => endPoint = value; }
    public float ScrollingSpeed { set => scrollingSpeed = value; }
    public bool Infinite { get => infinite; }

    private void Update()
    {
        Vector3 positionDelta = new Vector3(-Time.deltaTime * scrollingSpeed, 0, 0);
        transform.position -= positionDelta;
        ResetPosition();
    }

    private void ResetPosition()
    {
        if (transform.position.x < endPoint.position.x)
        {
            if (infinite)
            {
                Vector3 newPosition = new Vector3(spawnPoint.position.x, transform.position.y, transform.position.z);
                transform.position = newPosition;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
