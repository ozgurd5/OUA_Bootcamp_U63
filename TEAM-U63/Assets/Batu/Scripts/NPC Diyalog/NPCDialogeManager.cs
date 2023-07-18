using UnityEngine;

public class NPCDialogeManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject objectToOpen;
    
    private Dialogue dialogue;
    private Animator animator;

    private void Start()
    {
        dialogue = objectToOpen.GetComponentInChildren<Dialogue>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        if (col.name == "ArtistPlayer")
            col.transform.Find("ArtistCrosshairCanvas").Find("TalentIcons").gameObject.SetActive(false);
        else col.transform.Find("CoderCrosshairCanvas").Find("TalentIcons").gameObject.SetActive(false);
        
        
        objectToOpen.SetActive(true);
        dialogue.StartDialogue(col.gameObject);
        animator.SetBool("isTalking", true);
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        if (col.name == "ArtistPlayer")
            col.transform.Find("ArtistCrosshairCanvas").Find("TalentIcons").gameObject.SetActive(true);
        else
            col.transform.Find("CoderCrosshairCanvas").Find("TalentIcons").gameObject.SetActive(true);
        
        objectToOpen.SetActive(false);
        dialogue.ResetDialogue();
        animator.SetBool("isTalking", false);
    }
}