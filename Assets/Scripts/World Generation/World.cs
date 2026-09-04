using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class World : MonoBehaviour
{
    [SerializeField] private int worldSize = 16;
    [SerializeField] private TextureManagerAsset textureManager;
    [SerializeField] private FastNoiseAsset noiseAsset;
    [SerializeField] private PlayerController playerControllerReference;

    private Dictionary<Vector3Int, ChunkBehavior> chunks = new Dictionary<Vector3Int, ChunkBehavior>();

    private void Start()
    {
        // TODO: make it not lag the game when generating
        // coroutine maybe?
        GenerateWorld();

        playerControllerReference.AddUpdateActions(); // allow player gravity and movement after world gen
    }

    private void GenerateWorld()
    {
        for (int x = -worldSize; x <= worldSize; x++)
            for (int z = -worldSize; z <= worldSize; z++)
            {
                Vector3Int chunkPosition = new Vector3Int(x, 0, z);
                if (!chunks.ContainsKey(chunkPosition)) CreateChunk(chunkPosition);
            }
    }

    private void CreateChunk(Vector3Int chunkPosition)
    {
        GameObject chunkObject = new GameObject($"Chunk_{chunkPosition.x}_{chunkPosition.y}_{chunkPosition.z}");
        chunkObject.transform.position = new Vector3(
            chunkPosition.x * ChunkBehavior.Resolution,
            0,
            chunkPosition.z * ChunkBehavior.Resolution
        );
        
        ChunkBehavior chunkBehavior = chunkObject.AddComponent<ChunkBehavior>();
        // generates terrain on ChunkBehavior.Start()

        chunkBehavior.TextureManager = textureManager;
        chunkBehavior.NoiseAsset = noiseAsset;
        chunkBehavior.transform.SetParent(transform);
        
        chunks.Add(chunkPosition, chunkBehavior);
    }
}