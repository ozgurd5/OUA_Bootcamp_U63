using UnityEngine;

public class LaserTarget : MonoBehaviour
{
    public Material targetMaterial;
    public bool isArrived;
    public void Update()
    {
        if (isArrived)
        {
            Debug.Log("tamamlandı");
        }
    }
}