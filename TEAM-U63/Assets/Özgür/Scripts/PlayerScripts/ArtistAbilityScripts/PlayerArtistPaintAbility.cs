using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Responsible of painting ability of the artist</para>
/// </summary>
public class PlayerArtistPaintAbility : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private float paintRange = 5f;
    [SerializeField] private List<Material> puzzleMaterials;    //Assignment order must be RGB

    private string[] puzzlesTag = { "RedPuzzle", "GreenPuzzle", "BluePuzzle" };
    
    private PlayerController pc;
    private Camera cam;
    
    private int currentMaterialAndTagIndex;
    
    private Ray crosshairRay;
    private RaycastHit crosshairHit;

    private GameObject selectedCube;
    private MeshRenderer selectedCubeMeshRenderer;
    
    private void Awake()
    {
        pc = GetComponent<PlayerController>();
        cam = Camera.main;
    }

    /// <summary>
    /// <para>Casts ray for cubes</para>
    /// </summary>
    /// <returns>True if a ray hits a cube</returns>
    private bool CastRayForCubes()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);
        bool wasRayHit = Physics.Raycast(crosshairRay, out crosshairHit, paintRange);

        if (!wasRayHit) return false;
        
        Collider col = crosshairHit.collider; //Shorter return statement :p
        return col.CompareTag("RedPuzzle") || col.CompareTag("GreenPuzzle") || col.CompareTag("BluePuzzle");
    }

    /// <summary>
    /// <para>Paints cubes by changing their material and tag</para>
    /// </summary>
    private void ChangeCubeMaterialAndTag()
    {
        //Index will go like 1-2-3-1-2-3 on and on...
        currentMaterialAndTagIndex = (currentMaterialAndTagIndex + 1) % puzzleMaterials.Count;
        selectedCube.tag = puzzlesTag[currentMaterialAndTagIndex];
        
        //Updating mesh renderer materials in Unity is ultra protected for several long reasons
        //Long story short: We can not change a single element of the mesh renderer's materials array
        //We can only change the complete array by assign an array to it
        //So we must make our changes in a temporary copy array and assign it to mesh renderer material
        
        Material[] newCubeMaterials = selectedCubeMeshRenderer.materials;
        newCubeMaterials[0] = puzzleMaterials[currentMaterialAndTagIndex];
        selectedCubeMeshRenderer.materials = newCubeMaterials;
    }

    private void Update()
    {
        //Painting the cube works by 4 stages: input check - raycast - reaching mesh renderer - changing material and tag
        
        if (!pc.input.isSecondaryAbilityKeyDown) return;   
        if (!CastRayForCubes()) return;
        
        selectedCube = crosshairHit.collider.gameObject;
        selectedCubeMeshRenderer = selectedCube.GetComponent<MeshRenderer>();
        
        ChangeCubeMaterialAndTag();
    }
}