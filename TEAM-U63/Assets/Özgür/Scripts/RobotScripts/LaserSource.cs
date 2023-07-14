using UnityEngine;

//TODO: REPETITIVE CODE - COMBINE WITH ROBOT LASER MANAGER
public class LaserSource : MonoBehaviour
{
    [Header("Assign - Material Index - RGB")]
    [SerializeField] private int sourceMaterialIndex;
    
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
                
                //Red (0) source can only activate green (1) robot, green (1) source can only activate blue (2) robot..
                //..and blue (2) source can only activate red (0) robot
                if (sourceMaterialIndex + 1 == connectedRobotRlm.robotMaterialIndex) //R -> G -> B
                    connectedRobotRlm.OpenLaser();
                else if (sourceMaterialIndex - 2 == connectedRobotRlm.robotMaterialIndex) //B -> R
                    connectedRobotRlm.OpenLaser();
                else
                    connectedRobotRlm.CloseLaser();
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