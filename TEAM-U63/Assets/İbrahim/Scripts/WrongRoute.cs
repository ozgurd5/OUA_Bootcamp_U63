using UnityEngine;

public class WrongRoute : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject respawnPoint;

    private AudioSource aus;

    private void Awake()
    {
        aus = GetComponent<AudioSource>();
    } 

    private void OnTriggerEnter(Collider col)
    {
        //if (col.CompareTag("Player"))
        //{
        //    col.transform.position = respawnPoint.transform.position;
        //    aus.Play();
        //}
    }
}