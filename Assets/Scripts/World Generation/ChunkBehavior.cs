using System;
using UnityEngine;
using Unity.Collections;

public class ChunkBehavior : MonoBehaviour
{
    [SerializeField] private Material material;
    
    private static int resolution = 16;
    private static int height = 256;

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
        
        Vector3 size = new Vector3(resolution, height, resolution);
        Bounds bounds = new Bounds(size / 2, size);

        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Meshing.CreateChunkUInt32(resolution, height, meshDataArray, Allocator.Persistent);
        Mesh mesh = new Mesh
        {
            name = "cube",
            bounds = bounds,
        };
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }
}