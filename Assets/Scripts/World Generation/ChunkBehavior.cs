using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public class ChunkBehavior : MonoBehaviour
{
    [SerializeField] private FastNoiseAsset noiseAsset;
    [SerializeField] private TextureManagerAsset textureManager;
    
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
        meshRenderer.material = textureManager.BlockMaterial;

        await GetMeshAsync("custom mesh", volume, (mesh) =>
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        });
    }

    public static async Task GetMeshAsync(string meshName, Volume volume, Action<Mesh> callback)
    {
        if (volume.Length == 0) return;
        
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);

        await Task.Factory.StartNew(() =>
        {
            Meshing.CreateChunkUInt32(volume, meshDataArray, Allocator.Persistent);
        });

        Vector3 size = new Vector3(volume.Resolution, volume.Height, volume.Resolution);
        Bounds bounds = new Bounds(size / 2, size);

        Mesh mesh = new Mesh
        {
            name = meshName,
            bounds = bounds,
        };
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        callback(mesh);
    }
}