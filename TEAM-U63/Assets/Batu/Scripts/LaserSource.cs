using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSource : MonoBehaviour
{
    [SerializeField] Transform laserStartPoint;
    [SerializeField] public LineRenderer lineRenderer;
    Vector3 direction;

    void Start()
    {
        direction = laserStartPoint.forward; // A laser will emerge from the laser point forward.
        lineRenderer.positionCount = 2; // position of the laser after touching the object
        lineRenderer.SetPosition(0, laserStartPoint.position); // laser start
        
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(laserStartPoint.position, direction, out hit, Mathf.Infinity)) // it will stretch infinity as long as it doesn't hit an object
        {

            if (hit.collider.CompareTag("robot")) // will only interact with objects with a robot tag
            {
            }
            lineRenderer.SetPosition(1, hit.point); // When the laser hits something, it will come into contact with all kinds of objects and will not pass through.

        }
        else
        {
            lineRenderer.SetPosition(1, direction * 100); // if it doesn't hit anything the light will go 100 units
        }
    }
}
