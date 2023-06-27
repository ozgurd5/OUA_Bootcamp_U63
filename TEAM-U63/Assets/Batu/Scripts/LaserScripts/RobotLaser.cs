using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotLaser : MonoBehaviour
{
    [SerializeField] Transform laserStartPoint;
    [SerializeField] public LineRenderer lineRenderer;
    private MeshRenderer meshRenderer;
    private bool isGettingLaser;
    private GameObject LastRobot;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        lineRenderer.positionCount = 2;
        CloseLaser();
    }

    public void OpenLaser()
    {
        lineRenderer.enabled = true;
        isGettingLaser = true;
    }

    public void CloseLaser()
    {
        lineRenderer.enabled = false;
    }

    void Update() //robottan çıkan laser
    {
        Vector3 direction = laserStartPoint.forward;
        lineRenderer.material = meshRenderer.material;
        lineRenderer.SetPosition(0, laserStartPoint.position);

        if (!isGettingLaser)
        {
            CloseLaser();
        }
        
        RaycastHit hit;
        if (Physics.Raycast(laserStartPoint.position, direction, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("robot"))
            {
                LastRobot = hit.collider.gameObject;
                hit.collider.gameObject.GetComponent<RobotLaser>().OpenLaser();
            }
            else if(LastRobot != null)
            {
                LastRobot?.GetComponent<RobotLaser>().CloseLaser();
            }
            lineRenderer.SetPosition(0, laserStartPoint.position);
            lineRenderer.SetPosition(1, hit.point);
            
        }
        else
        {
            if(LastRobot != null)
            {
                LastRobot?.GetComponent<RobotLaser>().CloseLaser();
            }

            lineRenderer.SetPosition(0, laserStartPoint.position);
            lineRenderer.SetPosition(1, laserStartPoint.position + direction * 100);
            
        }
        
    }
}
