using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSource : MonoBehaviour
{
    [SerializeField] Transform laserStartPoint;
    [SerializeField] public LineRenderer lineRenderer;

    private GameObject LastRobot;
    void Update()
    {
        Vector3 direction = laserStartPoint.forward; // Update the direction based on the current rotation of the laserStartPoint

        RaycastHit hit;
        if (Physics.Raycast(laserStartPoint.position, direction, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("robot"))
            {
                LastRobot = hit.collider.gameObject;
                hit.collider.gameObject.GetComponent<RobotLaser>().OpenLaser();
            }
            else
            {
                LastRobot?.GetComponent<RobotLaser>().CloseLaser();
            }
            lineRenderer.SetPosition(0, laserStartPoint.position);
            lineRenderer.SetPosition(1, hit.point);
            
        }
        else
        {
            lineRenderer.SetPosition(0, laserStartPoint.position);
            lineRenderer.SetPosition(1, laserStartPoint.position + direction * 100);

            
        }

    }
}
