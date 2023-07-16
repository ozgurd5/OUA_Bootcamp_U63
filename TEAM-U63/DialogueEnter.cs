using UnityEngine;

public class DialogueEnter : MonoBehaviour
{
    public GameObject objectToOpen;
    private bool isOpen;

    private void Start()
    {
        isOpen = false;
        objectToOpen.SetActive(isOpen);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOpen = true;
            objectToOpen.SetActive(isOpen);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOpen = false;
            objectToOpen.SetActive(isOpen);
        }
    }
}
