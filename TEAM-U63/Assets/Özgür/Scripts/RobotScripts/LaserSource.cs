using UnityEngine;

public class LaserSource : MonoBehaviour
{
    private Transform laserStartPoint;
    private LineRenderer lr;
    
    private RobotLaserManager connectedRobotRlm;
    private RaycastHit laserSourceHit;

    private void Awake()
    {
        laserStartPoint = transform.Find("LaserStartPoint");
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, laserStartPoint.position);
    }

    void Update()
    {
        //If we hit something
        if (Physics.Raycast(laserStartPoint.position, laserStartPoint.forward, out laserSourceHit, Mathf.Infinity))
        {
            //Set the end of the laser hit object
            lr.SetPosition(1, laserSourceHit.point);
            
            //If we hit robot
            if (laserSourceHit.collider.CompareTag("robot"))
            {
                connectedRobotRlm = laserSourceHit.collider.gameObject.GetComponent<RobotLaserManager>();
                connectedRobotRlm.OpenLaser();
            }
            
            //If we hit something else //We need to prevent null ref ex in the beginning of the game
            else if (connectedRobotRlm != null) connectedRobotRlm.CloseLaser();
        }
        
        //If we don't hit something
        else
        {
            //Set the end of the laser 100 meter
            lr.SetPosition(1, laserStartPoint.position + laserStartPoint.forward * 100);
            
            //We need to prevent null ref ex in the beginning of the game
            if (connectedRobotRlm != null) connectedRobotRlm.CloseLaser();
        }
    }
}