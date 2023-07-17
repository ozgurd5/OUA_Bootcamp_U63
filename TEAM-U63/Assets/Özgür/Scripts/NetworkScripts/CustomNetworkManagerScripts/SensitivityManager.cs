using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SensitivityManager : MonoBehaviour
{
    [Header("Assign for Main Menu")]
    [SerializeField] private Slider sensSliderMM;
    [SerializeField] private TextMeshProUGUI sensTextMM;
    
    [Header("Assign for In Game")]
    [SerializeField] private Slider sensSliderIG;
    [SerializeField] private TextMeshProUGUI sensTextIG;

    [Header("No touch")]
    [SerializeField] private float sensValue = 8f;
    [SerializeField] private CinemachineFreeLook coderCam;
    [SerializeField] private CinemachineFreeLook artistCam;
    [SerializeField] private CinemachineFreeLook robotCam;
    [SerializeField] private string currentSceneName;
    
    private void Awake()
    {
        SceneManager.activeSceneChanged += (a, currentScene) =>
        {
            currentSceneName = currentScene.name;

            if (currentSceneName is not "MAIN_MENU" or "IntroCutscene" or "OutroCutscene")
            {
                coderCam = GameObject.Find("CoderPlayer").GetComponentInChildren<CinemachineFreeLook>();
                artistCam = GameObject.Find("ArtistPlayer").GetComponentInChildren<CinemachineFreeLook>();
            }
        };

        RobotManager.OnCurrentControlledRobotChanged += () =>
        {
            robotCam = RobotManager.currentControlledRobot.cam;
        };

        sensSliderIG.onValueChanged.AddListener((value) =>
        {
            sensValue = (float)Math.Round(value, 2) * 10;
            sensTextIG.text = sensValue.ToString("0.0");
        });
    }

    private void Start()
    {
        if (currentSceneName == "MAIN_MENU")
        {
            sensSliderMM.onValueChanged.AddListener((value) =>
            {
                sensValue = (float)Math.Round(value, 2) * 10;
                sensTextMM.text = sensValue.ToString("0.0");
            });
        }
    }

    private void Update()
    {
        if (currentSceneName is "MAIN_MENU" or "IntroCutscene" or "OutroCutscene") return;

        coderCam.m_YAxis.m_MaxSpeed = sensValue / 10;
        coderCam.m_XAxis.m_MaxSpeed = sensValue / 10 * 180;

        artistCam.m_YAxis.m_MaxSpeed = sensValue / 10;
        artistCam.m_XAxis.m_MaxSpeed = sensValue / 10 * 180;

        if (robotCam == null) return;
        robotCam.m_YAxis.m_MaxSpeed = sensValue / 10;
        robotCam.m_XAxis.m_MaxSpeed = sensValue / 10 * 180;
    }
}