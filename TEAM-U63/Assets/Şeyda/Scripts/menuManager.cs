using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class menuManager : MonoBehaviour
{
    public TMP_InputField username_inputField;
    public TMP_InputField roomid_inputField;
    public Button nextButton;
    public Button joinButton;
    public Button copyButton;
    public TMP_Text lobbyCode;
    public GameObject copyFeedback;

    public void quit()
    {
            Application.Quit();
            Debug.Log("Quit");
    } 
    
    void Start()
    { 
           nextButton.interactable = false;
           username_inputField.onValueChanged.AddListener(nextButtonReactivation);

           joinButton.interactable = false;
           roomid_inputField.onValueChanged.AddListener(joinButtonReactivation);

           copyButton.interactable = false;

           UnityRelayServiceManager.OnLobbyJoinCodeCreated += enableCopyCode;

    }
   
    
    public void nextButtonReactivation(string newValue)
    {  
          nextButton.interactable = !string.IsNullOrEmpty(newValue);      
    }


    public void joinButtonReactivation(string newValue)
    {       
        joinButton.interactable = !string.IsNullOrEmpty(newValue);
    }
 

    public void copyCode()
    {
        GUIUtility.systemCopyBuffer = lobbyCode.text;
    }


    public void enableCopyCode()
    {
        copyButton.interactable = true;
    }

    public void copiedFeedback()
    {
        copyFeedback.SetActive(true);
        Invoke(nameof(disableCopiedFeedback), 1.5f);
    }

    public void disableCopiedFeedback()
    {
        copyFeedback.SetActive(false);
    }
}
