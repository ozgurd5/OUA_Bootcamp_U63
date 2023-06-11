using Unity.Netcode;
using UnityEngine;

public class CubeScript : NetworkBehaviour
{
    public float interpolationValue = 0.5f;
    
    private void Update()
    {
        if (!IsHost) return;
        ClientInterpolationClientRPC(transform.position);
    }
    
    [ClientRpc]
    private void ClientInterpolationClientRPC(Vector3 targetPosition)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, interpolationValue);
    }
}
