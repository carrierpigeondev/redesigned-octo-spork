using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ChunkBehavior : MonoBehaviour
{
    // do need to be public because set by World
    public FastNoiseAsset NoiseAsset;
    public TextureManagerAsset TextureManager;
    
    private Volume volume;
    
    public static int Resolution = 256;
    public static int Height = 256;

    private async void Start()
    {
        float3 chunkPosition = transform.position;
        
        volume = new Volume(Resolution, Height);
        await volume.SetForeach((int3 localPosition) =>
        {
            float3 position = localPosition + chunkPosition;

            if (position.y < 0 || position.y >= Height)
                return TextureManager["air"];

            if (position.y == 100) return TextureManager["stone"];

            // rule: generate floor
            if (position.y == 0) return TextureManager["stone"];

            // rule: cave generation
            if (NoiseAsset.GetNoise(position) < 0.0f) return TextureManager["air"];
            
            // default
            return TextureManager["dirt"];
        });
        
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = TextureManager.BlockMaterial;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        await GetMeshAsync("custom mesh", volume, (mesh) =>
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            meshFilter.sharedMesh = mesh;
            meshCollider.sharedMesh = mesh;
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