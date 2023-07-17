using UnityEngine;

public class DialogueEnter : MonoBehaviour
{
    public GameObject objectToOpen;
    private bool isOpen;
    private Dialogue dialogue;
    private NPCTalk npcTalk;
    private Transform currentPlayerCrosshairCanvas;

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
            //crosshair canvas disable
            if (!collision.gameObject.GetComponent<PlayerData>().isLocal) return;

            currentPlayerCrosshairCanvas = collision.gameObject.GetComponent<CrosshairManager>().crosshairCanvas;
            currentPlayerCrosshairCanvas.gameObject.SetActive(false); //crosshair canvas disable
            
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
            
            //crosshair canvas enable
            currentPlayerCrosshairCanvas.gameObject.SetActive(true); //crosshair canvas enable

        }
    }
}