using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAnalyticsManager : MonoBehaviour
{
    [Header("Scene Info")]
    [SerializeField] private string currentSceneName;
    [SerializeField] private string previousSceneName;
    public bool isFirstIslandCompleted;
    public bool isSecondIslandCompleted;

    [Header("Play Times")]
    public float firstIslandGeneralPlayTime;
    public float secondIslandGeneralPlayTime;

    private void Awake()
    {
        SceneManager.activeSceneChanged += (previousScene, currentScene) =>
        {
            //not working, idk why
            //previousSceneName = previousScene.name;
            
            previousSceneName = currentSceneName;
            currentSceneName = currentScene.name;

            if (currentSceneName == "Main Island")
            {
                GameObject.Find("AnalyticsBoard").GetComponentInChildren<TextMeshPro>().text = 
                $"First Island Play Time: {(int)firstIslandGeneralPlayTime} seconds\nSecond Island Play Time: " +
                $"{(int)secondIslandGeneralPlayTime} seconds";
                
                if (previousSceneName == "Island 1") isFirstIslandCompleted = true;
                else if (previousSceneName == "Island 2") isSecondIslandCompleted = true;
            }
        };
    }

    private void Update()
    {
        if (currentSceneName == "Island 1") IncreasePlayTime(ref firstIslandGeneralPlayTime);
        else if (currentSceneName == "Island 2") IncreasePlayTime(ref secondIslandGeneralPlayTime);
    }
    
    
    private void IncreasePlayTime(ref float playTime)
    {
        playTime += Time.deltaTime;
    }
}
