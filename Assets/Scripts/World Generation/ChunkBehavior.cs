using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public class ChunkBehavior : MonoBehaviour
{
    [SerializeField] private FastNoiseAsset noiseAsset;
    [SerializeField] private Material material;
    
    private Volume volume;
    
    public static int resolution = 16;
    public static int height = 256;

    private async void Start()
    {
        volume = new Volume(resolution, height);
        await volume.SetForeach((int3 localPosition) =>
        {
            if (noiseAsset.GetNoise(localPosition) < 0.0f) return 0;
            return 1;
        });
        
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
        
        Vector3 size = new Vector3(resolution, height, resolution);
        Bounds bounds = new Bounds(size / 2, size);

        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Meshing.CreateChunkUInt32(volume, meshDataArray, Allocator.Persistent);
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