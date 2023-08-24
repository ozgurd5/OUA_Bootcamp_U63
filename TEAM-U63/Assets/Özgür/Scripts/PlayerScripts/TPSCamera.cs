using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    [SerializeField] private Transform followTargetTransform;
    [SerializeField] private Transform lookAtTargetTransform;

    private void Update()
    {
        transform.position = followTargetTransform.position;
        transform.LookAt(lookAtTargetTransform);
    }
}
