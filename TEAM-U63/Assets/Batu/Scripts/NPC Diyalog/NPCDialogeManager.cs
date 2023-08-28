using UnityEngine;

public class NPCDialogeManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject objectToOpen;
    
    private Dialogue dialogue;
    private Animator animator;

    private PlayerData artistPlayerData;
    private PlayerData coderPlayerData;
    
    private GameObject artistTalentIcons;
    private GameObject coderTalentIcons;

    private void Start()
    {
        dialogue = objectToOpen.GetComponentInChildren<Dialogue>();
        animator = GetComponent<Animator>();

        artistPlayerData = GameObject.Find("ArtistPlayer").GetComponent<PlayerData>();
        coderPlayerData = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
        
        artistTalentIcons = artistPlayerData.transform.Find("ArtistCrosshairCanvas").Find("TalentIcons").gameObject;
        coderTalentIcons = coderPlayerData.transform.Find("CoderCrosshairCanvas").Find("TalentIcons").gameObject;

        artistPlayerData.OnLocalStatusChanged += () => { CloseDialogue(artistPlayerData.gameObject); };
        coderPlayerData.OnLocalStatusChanged += () => { CloseDialogue(coderPlayerData.gameObject); };
    }

    private void OpenDialogue(GameObject talkingPlayer)
    {
        if (talkingPlayer.name == "ArtistPlayer") artistTalentIcons.SetActive(false);
        else coderTalentIcons.SetActive(false);
        
        objectToOpen.SetActive(true);
        dialogue.StartDialogue(talkingPlayer);
        animator.SetBool("isTalking", true);
    }

    private void CloseDialogue(GameObject talkingPlayer)
    {
        if (talkingPlayer.name == "ArtistPlayer") artistTalentIcons.SetActive(true);
        else coderTalentIcons.SetActive(true);
        
        objectToOpen.SetActive(false);
        dialogue.ResetDialogue();
        animator.SetBool("isTalking", false);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        if (col.name == "ArtistPlayer" && artistPlayerData.isLocal) OpenDialogue(artistPlayerData.gameObject);
        else if (coderPlayerData.isLocal) OpenDialogue(coderPlayerData.gameObject);
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        if (col.name == "ArtistPlayer" && artistPlayerData.isLocal) CloseDialogue(col.gameObject);
        else if (coderPlayerData.isLocal) CloseDialogue(col.gameObject);
    }
}