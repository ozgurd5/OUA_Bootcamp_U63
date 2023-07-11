using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CubeManager : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private List<Material> puzzleMaterials;    //Assignment order must be RGB
    private string[] puzzlesTag = { "RedPuzzle", "GreenPuzzle", "BluePuzzle" };
    
    public bool isGrabbed;          //PlayerGrabbing.cs
    public int materialAndTagIndex; //PlayerArtistPaintAbility.cs

    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// <para>Paints cubes by changing their material and tag</para>
    /// </summary>
    public void PaintCube()
    {
        //Index will go like 1-2-3-1-2-3 on and on...
        materialAndTagIndex = (materialAndTagIndex + 1) % puzzleMaterials.Count;
        UpdateMaterialAndTag();
        UpdateCubeStatusClientRpc(materialAndTagIndex);
        if (!IsHost) UpdateCubeStatusServerRpc(materialAndTagIndex);
    }

    private void UpdateMaterialAndTag()
    {
        tag = puzzlesTag[materialAndTagIndex];
        
        //Updating mesh renderer materials in Unity is ultra protected for several long reasons
        //Long story short: We can not change a single element of the mesh renderer's materials array
        //We can only change the complete array by assign an array to it
        //So we must make our changes in a temporary copy array and assign it to mesh renderer material
        
        Material[] newCubeMaterials = mr.materials;
        newCubeMaterials[0] = puzzleMaterials[materialAndTagIndex];
        mr.materials = newCubeMaterials;
    }

    [ClientRpc]
    private void UpdateCubeStatusClientRpc(int newMaterialAndTagIndex)
    {
        if (IsHost) return;
        materialAndTagIndex = newMaterialAndTagIndex;
        UpdateMaterialAndTag();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateCubeStatusServerRpc(int newMaterialAndTagIndex)
    {
        materialAndTagIndex = newMaterialAndTagIndex;
        UpdateMaterialAndTag();
    }
}