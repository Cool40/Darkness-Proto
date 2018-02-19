using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertToSkinnedMesh : MonoBehaviour {

    [ContextMenu("Convert to skinned mesh")]
    void Convert()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        skinnedMeshRenderer.sharedMesh = meshFilter.sharedMesh;
        skinnedMeshRenderer.sharedMaterials = meshRenderer.sharedMaterials;

        DestroyImmediate(meshFilter);
        DestroyImmediate(meshRenderer);
        DestroyImmediate(this);
    }
}
