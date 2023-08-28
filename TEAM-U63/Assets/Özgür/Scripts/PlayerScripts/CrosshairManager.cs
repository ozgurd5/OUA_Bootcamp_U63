using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Responsible from crosshair opacity and raycast</para>
/// </summary>
public class CrosshairManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float cubeRange = 10f;
    [SerializeField] private float robotRange = 30f;
    [SerializeField] [Range(0, 1)] private float opacity = 0.3f;

    [Header("Info - No Touch")]
    public bool isLookingAtCube;
    public bool isLookingAtRobot;
    public Ray crosshairRay;
    public RaycastHit crosshairHit;

    private PlayerData pd;
    private PlayerStateData psd;
    public Transform crosshairCanvas;
    private Image crosshairImage;
    private Camera cam;

    private bool crosshairVisibilityCondition;
    private Color temporaryColor;

    private void Awake()
    {
        pd = GetComponentInParent<PlayerData>();
        psd = GetComponentInParent<PlayerStateData>();
        crosshairCanvas = transform.parent;
        crosshairImage = GetComponent<Image>();
        cam = Camera.main; 
        
        pd.OnLocalStatusChanged += ToggleCanvas;
        ToggleCanvas();
    }
    
    private void ToggleCanvas()
    {
        crosshairCanvas.gameObject.SetActive(pd.isLocal);
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H)) Debug.Log(crosshairHit.collider.name);
        #endif
        
        //The reason why we have two methods is their ranges are different
        CastRayForCubes();
        CastRayForRobots();

        crosshairVisibilityCondition = isLookingAtCube || psd.isGrabbing || isLookingAtRobot;
        
        //Just like the mesh renderer example, we can not directly change crosshairImage.color.a
        //We can only assign a color variable to it. Therefore we need a temporary color variable..
        //..to make changes upon and finally assign it
        
        temporaryColor = crosshairImage.color;
        if (crosshairVisibilityCondition) temporaryColor.a = 1f;
        else temporaryColor.a = opacity;
        crosshairImage.color = temporaryColor;
    }
    
    private void CastRayForCubes()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);

        if (!Physics.Raycast(crosshairRay, out crosshairHit, cubeRange))
        {
            isLookingAtCube = false;
            return;
        }
        
        isLookingAtCube = crosshairHit.collider.CompareTag("RedPuzzle") ||
                          crosshairHit.collider.CompareTag("GreenPuzzle") ||
                          crosshairHit.collider.CompareTag("BluePuzzle");
    }
    
    private void CastRayForRobots()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);

        if (!Physics.Raycast(crosshairRay, out crosshairHit, robotRange))
        {
            isLookingAtRobot = false;
            return;
        }
        
        isLookingAtRobot = crosshairHit.collider.CompareTag("Robot");
    }
}
