using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Responsible from crosshair opacity and raycast</para>
/// </summary>
public class CrosshairManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float interactionRange = 7f;
    [SerializeField] [Range(0, 1)] private float opacity = 0.3f;

    [Header("Info - No Touch")]
    public bool isLookingAtCube;
    public bool isLookingAtRobot;
    public Ray crosshairRay;
    public RaycastHit crosshairHit;

    private PlayerData pd;
    private PlayerStateData psd;
    private Image crosshairImage;
    private Camera cam;

    private bool crosshairVisibilityCondition;
    private Color temporaryColor;

    private void Awake()
    {
        pd = GetComponentInParent<PlayerData>();
        psd = GetComponentInParent<PlayerStateData>();
        crosshairImage = GetComponent<Image>();
        cam = Camera.main;

        pd.OnLocalStatusChanged += ToggleCrosshair;
        ToggleCrosshair();
    }

    /// <summary>
    /// <para>Toggles crosshair on or of according to local status</para>
    /// </summary>
    private void ToggleCrosshair()
    {
        crosshairImage.enabled = pd.isLocal;
    }

    private void Update()
    {
        //TODO: debug purpose, remove before build
        if (Input.GetKeyDown(KeyCode.G)) Debug.Log(crosshairHit.collider.gameObject.name);

        if (CastRayFromCrosshair())
        {
            isLookingAtCube = crosshairHit.collider.CompareTag("RedPuzzle") ||
                              crosshairHit.collider.CompareTag("GreenPuzzle") ||
                              crosshairHit.collider.CompareTag("BluePuzzle");

            isLookingAtRobot = crosshairHit.collider.CompareTag("robot");
        }

        else    //If in the up won't work if raycast return null
        {
            isLookingAtCube = false;
            isLookingAtRobot = false;
        }

        crosshairVisibilityCondition = isLookingAtCube || psd.isGrabbing || isLookingAtRobot;
        
        //Just like the mesh renderer example, we can not directly change crosshairImage.color.a
        //We can only assign a color variable to it. Therefore we need a temporary color variable..
        //..to make changes upon and finally assign it
        
        temporaryColor = crosshairImage.color;
        if (crosshairVisibilityCondition) temporaryColor.a = 1f;
        else temporaryColor.a = opacity;
        crosshairImage.color = temporaryColor;
    }
    
    /// <summary>
    /// <para>Casts ray for cubes</para>
    /// </summary>
    /// <returns>True if a ray hits a cube</returns>
    private bool CastRayFromCrosshair()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);
        return Physics.Raycast(crosshairRay, out crosshairHit, interactionRange);
    }
}
