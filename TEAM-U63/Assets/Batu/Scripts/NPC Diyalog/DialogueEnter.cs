using UnityEngine;

public class DialogueEnter : MonoBehaviour
{
    public GameObject objectToOpen;
    private bool isOpen;
    private Dialogue dialogue;
    private NPCTalk npcTalk;


    private void Start()
    {
        isOpen = false;
        objectToOpen.SetActive(isOpen);
        dialogue = objectToOpen.GetComponentInChildren<Dialogue>();
        npcTalk = objectToOpen.GetComponentInChildren<NPCTalk>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOpen = true;

            objectToOpen.SetActive(isOpen);
            dialogue.StartDialogue(); // Start the dialogue
            
            npcTalk.StartTalking();
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOpen = false;

            objectToOpen.SetActive(isOpen);
            dialogue.ResetDialogue(); // Reset the dialogue state
            
            npcTalk.StopTalking();
        }
    }
}