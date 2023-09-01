using UnityEngine;

public class LabEnder : MonoBehaviour
{
    [SerializeField] private bool canEndLab;
    
    private PlayerData coderPlayerData;
    private PlayerData artistPlayerData;

    private Vector3 offset = new Vector3(0f, 0f, 5f);

    private void Awake()
    {
        coderPlayerData = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
        artistPlayerData = GameObject.Find("ArtistPlayer").GetComponent<PlayerData>();

        ScaleController.OnScaleCompleted += ScaleCompleted;
    }

    private void ScaleCompleted(bool isCompleted)
    {
        if (isCompleted) canEndLab = true;
    }

    private void Update()
    {
        if (canEndLab && Input.GetKeyDown(KeyCode.N) && Input.GetKey(KeyCode.RightShift))
        {
            if (coderPlayerData.isLocal)
            {
                coderPlayerData.transform.position = transform.position + offset;
            }

            else if (artistPlayerData.isLocal)
            {
                artistPlayerData.transform.position = transform.position - offset;
            }
        }
    }

    private void OnDestroy()
    {
        ScaleController.OnScaleCompleted -= ScaleCompleted;
    }
}
