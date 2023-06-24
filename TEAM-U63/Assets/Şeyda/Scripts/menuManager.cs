using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class menuManager : MonoBehaviour
{
    public TMP_InputField username_inputField;
    public TMP_InputField roomid_inputField;
    public Button nextButton;
    public Button joinButton;

    public void quit()
        {
            Application.Quit();
            Debug.Log("Quit");
        }
      
    void Start()
       { 
           nextButton.interactable = false;
           username_inputField.onValueChanged.AddListener(nextButtonDeactivation);

           joinButton.interactable = false;
           roomid_inputField.onValueChanged.AddListener(joinButtonDeactivation);
        }
       
    public void nextButtonDeactivation(string newValue)
       {
           
           nextButton.interactable = !string.IsNullOrEmpty(newValue);
           
        }
    public void joinButtonDeactivation(string newValue)
    {
        
        joinButton.interactable = !string.IsNullOrEmpty(newValue);
    }

}
