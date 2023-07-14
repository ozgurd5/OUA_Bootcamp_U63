using System.Collections;
using UnityEngine;

public class AiRoute : MonoBehaviour
{
    //TODO: FIND A BETTER SOLUTION
    [Header("Robot ID for starting waypoint")]
    [SerializeField] private int robotID;
    
    [Header("Assign")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float waitTime = 0.1f;
    [SerializeField] private float turnSpeed = 200f;
    [SerializeField] private Transform routeHolder;

    private void Start()
    {
        Vector3[] wayPoints = new Vector3 [routeHolder.childCount];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = routeHolder.GetChild(i).position;
            wayPoints[i] = new Vector3(wayPoints[i].x, transform.position.y, wayPoints[i].z);
        }

        StartCoroutine(FollowRoute(wayPoints));
    }

    IEnumerator FollowRoute(Vector3[] waypoints)
    {
        transform.position = waypoints[robotID];

        int targetWaypointIndex = 1;
        Vector3 targetWayPoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWayPoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, speed * Time.deltaTime);

            if (transform.position == targetWayPoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWayPoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWayPoint));
            }

            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y ,targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = routeHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in routeHolder)
        {
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        
        Gizmos.DrawLine(previousPosition , startPosition);
    }
}
