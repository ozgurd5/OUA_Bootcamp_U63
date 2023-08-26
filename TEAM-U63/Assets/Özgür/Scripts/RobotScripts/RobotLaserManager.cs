using UnityEngine;

public class RobotLaserManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioSource aus;
    
    private Transform laserStartPoint;
    private LineRenderer lr;
    private MeshRenderer mr;
    
    private bool isGettingLaser;
    private RaycastHit laserSourceHit;
    private RobotLaserManager connectedRobotRlm;
    private LaserTarget connectedLaserTarget;
    
    //TODO: FIND A BETTER SOLUTION
    private bool isLaserSoundPlayed;

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
        //TODO FIND A BETTER SOLUTION: NOT FOR AUS, BUT CONTINUOUS METHOD CALL
        if (!isLaserSoundPlayed) aus.Play();
        isLaserSoundPlayed = true;
        
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
        
        //
        isLaserSoundPlayed = false;
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
            if (laserSourceHit.collider.CompareTag("Robot") && isGettingLaser)
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
            
            else if (laserSourceHit.collider.CompareTag("LaserTarget") && isGettingLaser)
            {
                connectedLaserTarget = laserSourceHit.collider.GetComponent<LaserTarget>();
                connectedLaserTarget.CheckLaserTargetHit(robotMaterialIndex);
            }
            
            //If we hit something else //We need to prevent null ref ex in the beginning of the game
            else if (connectedRobotRlm != null) connectedRobotRlm.CloseLaser();
            
            //TODO: find a better solution
            //If we hit something else //We need to prevent null ref ex in the beginning of the game
            else if (connectedLaserTarget != null) connectedLaserTarget.CheckLaserTargetHit(10);
        }
        
        //If we don't hit something
        else
        {
            //Set the end of the laser 100 meter
            lr.SetPosition(1, laserStartPoint.position + laserStartPoint.forward * 100);
            
            //We need to prevent null ref ex in the beginning of the game
            if (connectedRobotRlm != null) connectedRobotRlm.CloseLaser();
            
            //TODO: find a better solution
            //We need to prevent null ref ex in the beginning of the game
            if (connectedLaserTarget != null) connectedLaserTarget.CheckLaserTargetHit(10);
        }
    }
}