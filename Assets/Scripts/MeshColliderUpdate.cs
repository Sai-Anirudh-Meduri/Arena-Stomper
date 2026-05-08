using UnityEngine;

public class MeshColliderUpdate : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    MeshCollider meshCollider;

    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void LateUpdate()
    {
        Mesh tempMesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(tempMesh);
        meshCollider.sharedMesh = tempMesh;
    }
}
