using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Transform followTargetTransform;
    [SerializeField] private Transform lookAtTargetTransform;

    private void Update()
    {
        transform.position = followTargetTransform.position;
        transform.LookAt(lookAtTargetTransform);
    }
}
