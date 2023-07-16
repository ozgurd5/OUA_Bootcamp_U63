using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SensitivityManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Slider sensSlider;
    [SerializeField] private TextMeshProUGUI sensText;

    [Header("No touch")]
    [SerializeField] private float sensValue = 8f;
    [SerializeField] private CinemachineFreeLook coderCam;
    [SerializeField] private CinemachineFreeLook artistCam;
    [SerializeField] private CinemachineFreeLook robotCam;
    [SerializeField] private string currentSceneName;
    
    private void Awake()
    {
        sensSlider.onValueChanged.AddListener((value) =>
        {
            sensValue = (float)Math.Round(value, 2) * 10;
            sensText.text = sensValue.ToString("0.0");
        });

        SceneManager.activeSceneChanged += (a, currentScene) => currentSceneName = currentScene.name;

        RobotManager.OnCurrentControlledRobotChanged += () =>
        {
            robotCam = RobotManager.currentControlledRobot.cam;
            
            robotCam.m_YAxis.m_MaxSpeed = sensValue / 10;
            robotCam.m_XAxis.m_MaxSpeed = sensValue / 10 * 180;
        };
    }

    private void Update()
    {
        if (currentSceneName == "MAIN_MENU") return;
        
        coderCam = GameObject.Find("CoderPlayer").GetComponentInChildren<CinemachineFreeLook>();
        artistCam = GameObject.Find("ArtistPlayer").GetComponentInChildren<CinemachineFreeLook>();

        coderCam.m_YAxis.m_MaxSpeed = sensValue / 10;
        coderCam.m_XAxis.m_MaxSpeed = sensValue / 10 * 180;

        artistCam.m_YAxis.m_MaxSpeed = sensValue / 10;
        artistCam.m_XAxis.m_MaxSpeed = sensValue / 10 * 180;
    }
}