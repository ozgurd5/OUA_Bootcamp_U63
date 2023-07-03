using DG.Tweening;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public void OpenDoor()
    {
        //gameObject.SetActive(false);
        transform.DOMoveY(transform.position.y - 2f, 1f);
    }
}