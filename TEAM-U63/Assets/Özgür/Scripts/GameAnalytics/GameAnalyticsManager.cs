using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class GameAnalyticsManager : MonoBehaviour
{
    private int testEventNumber;
    
    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            
            //IDK why we need this list
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            Debug.Log(e);
        }
    }
    
    private void SendTestEvent()
    {
        testEventNumber++;
        
        Dictionary<string, object> testEventParameters = new Dictionary<string, object>()
        {
            { "testParameter", testEventNumber }
        };

        AnalyticsService.Instance.CustomData("testEvent", testEventParameters);
        AnalyticsService.Instance.Flush();
        
    }
}
