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
    public FastNoiseAsset PrimaryNoiseAsset;
    public FastNoiseAsset StrataNoiseAsset;
    
    public TextureManagerAsset TextureManager;
    
    private Volume volume;
    
    public static int Resolution = 32;
    public static int Height = 192;
    public static float BlockSize = 0.5f;

    public async Task GenerateAsync(Material blockMaterial)
    {
        float3 chunkPosition = transform.position;
        
        // for performance, rather than doing lookups for every single texturemanager index
        // assign them to bytes outside of the loop
        byte blockAir = TextureManager["air"];
        byte blockRegolith = TextureManager["regolith"];
        byte blockBreccia = TextureManager["breccia"];
        byte blockAnorthosite = TextureManager["anorthosite"];
        
        volume = new Volume(Resolution, Height);
        
        await volume.SetForeach((int3 localPosition) =>
        {
            // rule: fill in blocks above and below world as air
            if (localPosition.y < 0 || localPosition.y >= Height)
                return blockAir;

            // rule: generate floor
            if (localPosition.y == 0) return blockAnorthosite;

            
            float3 worldPosition = chunkPosition + (float3)localPosition * BlockSize;
            
            // rule: cave generation
            if (PrimaryNoiseAsset.GetNoise(worldPosition) < 0.0f) return blockAir;
            
            // rule: strata running throughout terrain
            if (StrataNoiseAsset.GetNoise(worldPosition) > 0.0f)
            {
                if (localPosition.y >= 80) return blockRegolith;
                if (localPosition.y <= 30) return blockAnorthosite;
            };
            
            // default
            return blockBreccia;
        });
        
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = blockMaterial;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        Mesh mesh = await GetMeshAsync(
            $"ChunkMesh_{transform.position.x}_{transform.position.z}",
            volume
        );
        
        // component handling outside of async
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    public static async Task<Mesh> GetMeshAsync(string meshName, Volume volume)
    {
        if (volume.Length == 0) return null;
        
        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);

        await Task.Run(() =>
        {
            Meshing.CreateChunkUInt32(volume, meshDataArray, Allocator.Persistent);
        });

        Vector3 size = new Vector3(volume.Resolution, volume.Height, volume.Resolution) * BlockSize;
        Bounds bounds = new Bounds(size / 2, size);

        Mesh mesh = new Mesh
        {
            name = meshName,
            bounds = bounds,
        };
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);

        return mesh;
    }
}