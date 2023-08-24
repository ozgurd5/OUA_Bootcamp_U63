using System;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
     [Header("Assign")]
     [SerializeField] private GameObject settings;
     private Canvas pauseMenuCanvas;
     
     private PlayerData coderPlayerData;
     private PlayerData artistPlayerData;
     private PlayerInputManager pim;
     private PlayerStateData psd;
     private PlayerStateData.PlayerMainState previousState;
     
     private bool isPlayerDataSet; //We can get it one frame after start
     
     private void Awake()
     {
          pauseMenuCanvas = GetComponent<Canvas>();
          
          CloseCursor();
     }

     private void Update()
     {
          if (!isPlayerDataSet) //We can get it one frame after start
          {
               GetPlayerData();
               isPlayerDataSet = pim && psd;
          }
          
          if (pim.isPauseKeyDown)
          {
               pauseMenuCanvas.enabled = !pauseMenuCanvas.enabled;
               
               if (pauseMenuCanvas.enabled)
               {
                    OpenCursor();
                    
                    previousState = psd.currentMainState;
                    psd.currentMainState = PlayerStateData.PlayerMainState.RobotControllingState; //Player freezes in this state
               }

               else
               {
                    CloseCursor();
                    CloseMainMenu();
               }
          }
     }

     private void GetPlayerData()
     {
          coderPlayerData = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
          artistPlayerData = GameObject.Find("ArtistPlayer").GetComponent<PlayerData>();
          
          if (coderPlayerData.isLocal)
          {
               pim = coderPlayerData.GetComponent<PlayerInputManager>();
               psd = coderPlayerData.GetComponent<PlayerStateData>();
          }
          
          else if (artistPlayerData.isLocal)
          {
               pim = artistPlayerData.GetComponent<PlayerInputManager>();
               psd = artistPlayerData.GetComponent<PlayerStateData>();
          }
     }

     private void OpenCursor()
     {
          Cursor.visible = true;
          Cursor.lockState = CursorLockMode.None;
     }

     private void CloseCursor()
     {
          Cursor.visible = false;
          Cursor.lockState = CursorLockMode.Locked;
     }

     
     public void ExitGame()
     {
          Application.Quit();
     }

     public void CloseMainMenu()
     {
          pauseMenuCanvas.enabled = false;
          settings.SetActive(false);
          psd.currentMainState = previousState;
     }
}
