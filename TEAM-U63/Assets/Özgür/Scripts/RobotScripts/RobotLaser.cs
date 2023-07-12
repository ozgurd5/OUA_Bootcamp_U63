using UnityEngine;

public class RobotLaser : MonoBehaviour
{
    [SerializeField] Transform laserStartPoint;
    [SerializeField] public LineRenderer lineRenderer;
    private MeshRenderer meshRenderer;
    private bool isGettingLaser;
    private GameObject LastRobot;

    void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
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
            //Debug.Log(hit.collider.name);
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
            
            if (hit.collider.CompareTag("LaserTarget"))
            {
                
                LaserTarget laserTarget = hit.collider.gameObject.GetComponent<LaserTarget>();
                Material targetMaterial = laserTarget.targetMaterial;
                
                //Debug.Log(targetMaterial.name + " target");
                //Debug.Log(meshRenderer.material.name + " meshRenderer");

                if (targetMaterial.Equals(meshRenderer.material))
                {
                    laserTarget.isArrived = true;
                }

            }
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