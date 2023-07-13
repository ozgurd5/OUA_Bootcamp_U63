using UnityEngine;

public class RobotLaserManager : MonoBehaviour
{
    private Transform laserStartPoint;
    private LineRenderer lr;
    private MeshRenderer mr;
    
    private RobotLaserManager connectedRobotRlm;
    private RaycastHit laserSourceHit;
    private bool isGettingLaser;

    //We can not compare materials by their value using == operator, so we have to compare integers
    //Read UpdateRobotMaterialIndex summary
    public int robotMaterialIndex;
    private RobotManager rm;

    void Awake()
    {
        laserStartPoint = transform.Find("LaserStartPoint");
        mr = transform.Find("BODY").GetComponent<MeshRenderer>();
        lr = GetComponent<LineRenderer>();

        //Read UpdateRobotMaterialIndex summary
        rm = GetComponent<RobotManager>();
        rm.OnRobotPainted += UpdateRobotMaterialIndex;
    }

    public void OpenLaser()
    {
        lr.enabled = true;
        isGettingLaser = true;
        lr.material = mr.material;
    }

    public void CloseLaser()
    {
        //When the robot close it's laser, it must also close the connected robot's laser too
        if (connectedRobotRlm != null) connectedRobotRlm.CloseLaser();
        
        isGettingLaser = false;
        lr.enabled = false;
    }

    /// <summary>
    /// <para>Updates robotMaterialIndex in this script whenever it's changed in RobotManager.cs when the robot is painted</para>
    /// <para>We can simply make it public in there but I don't think we should reach RobotManager.cs whenever a robot hit
    /// another robot. Code design choice, may change</para>
    /// </summary>
    private void UpdateRobotMaterialIndex(int newRobotMaterialIndex)
    {
        robotMaterialIndex = newRobotMaterialIndex;
    }
    
    void Update()
    {
        lr.SetPosition(0, laserStartPoint.position);
        
        //If we hit something
        if (Physics.Raycast(laserStartPoint.position, laserStartPoint.forward, out laserSourceHit, Mathf.Infinity))
        {
            //Set the end of the laser hit object
            lr.SetPosition(1, laserSourceHit.point);
            
            //If we hit robot
            if (laserSourceHit.collider.CompareTag("robot") && isGettingLaser)
            {
                connectedRobotRlm = laserSourceHit.collider.gameObject.GetComponent<RobotLaserManager>();
                
                //Red (0) robot can only activate green (1) robot, green (1) robot can only activate blue (2) robot..
                //..and blue (2) robot can only activate red (0) robot
                if (robotMaterialIndex + 1 == connectedRobotRlm.robotMaterialIndex) //R -> G -> B
                    connectedRobotRlm.OpenLaser();
                else if (robotMaterialIndex -2 == connectedRobotRlm.robotMaterialIndex) //B -> R
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