using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotLaser : MonoBehaviour
{
    [SerializeField] Transform laserStartPoint;
    [SerializeField] public LineRenderer lineRenderer;
    private Material robotMaterial;

    void Start()
    {
        robotMaterial = GetComponent<MeshRenderer>().material;
        lineRenderer.positionCount = 2;
        CloseLaser();
    }

    public void OpenLaser()
    {
        if (lineRenderer != null && robotMaterial != null)
        {
            lineRenderer.enabled = true;
        }
    }

    public void CloseLaser()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        Vector3 direction = laserStartPoint.forward;
        lineRenderer.materials[0] = robotMaterial;
        lineRenderer.SetPosition(0, laserStartPoint.position);

        RaycastHit hit;
        if (Physics.Raycast(laserStartPoint.position, direction, out hit, Mathf.Infinity))
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, laserStartPoint.position + direction * 100);
        }
    }
}
