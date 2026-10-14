using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Random = System.Random;

public class World : MonoBehaviour
{
    [SerializeField] private int worldSize = 16;
    [SerializeField] private int maxConcurrentChunkGenerations = 4;
    
    [SerializeField] private TextureManagerAsset textureManager;
    [SerializeField] private FastNoiseAsset noiseAsset;
    [SerializeField] private FastNoiseAsset strataNoiseAsset;
    [SerializeField] private PlayerController playerControllerReference;
    [SerializeField] private GameObject shopZoneObject;

    private Dictionary<Vector3Int, ChunkBehavior> chunks = new Dictionary<Vector3Int, ChunkBehavior>();

    private async void Start()
    {
        Material blockMaterial = textureManager.BlockMaterial;
        await GenerateWorldAsync(blockMaterial);

        // Move the player
        MovePlayerToLocation();

        // Spawn the shop
        SpawnShop();

        // Spawn enemies
        SpawnEnemies();

        

        playerControllerReference.AddUpdateActions(); // allow player gravity and movement after world gen
    }

    private async Task GenerateWorldAsync(Material blockMaterial)
    {
        List<Task> activeWorldGenerationTasks = new List<Task>(maxConcurrentChunkGenerations);
        
        for (int x = -worldSize; x <= worldSize; x++)
        for (int z = -worldSize; z <= worldSize; z++)
        {
            Vector3Int chunkPosition = new Vector3Int(x, 0, z);
            if (chunks.ContainsKey(chunkPosition)) continue;

            ChunkBehavior chunkBehavior = CreateChunk(chunkPosition);
            activeWorldGenerationTasks.Add(chunkBehavior.GenerateAsync(blockMaterial));

            if (activeWorldGenerationTasks.Count < maxConcurrentChunkGenerations) continue;
            
            Task completed = await Task.WhenAny(activeWorldGenerationTasks);
            activeWorldGenerationTasks.Remove(completed);
            await completed;
        }
        
        await Task.WhenAll(activeWorldGenerationTasks);
    }

    private ChunkBehavior CreateChunk(Vector3Int chunkPosition)
    {
        GameObject chunkObject = new GameObject($"Chunk_{chunkPosition.x}_{chunkPosition.y}_{chunkPosition.z}");
        
        chunkObject.transform.SetParent(transform);
        chunkObject.transform.position = new Vector3(
            chunkPosition.x * ChunkBehavior.Resolution * ChunkBehavior.BlockSize,
            0,
            chunkPosition.z * ChunkBehavior.Resolution * ChunkBehavior.BlockSize
        );
        
        ChunkBehavior chunkBehavior = chunkObject.AddComponent<ChunkBehavior>();

        chunkBehavior.TextureManager = textureManager;
        chunkBehavior.PrimaryNoiseAsset = noiseAsset;
        chunkBehavior.StrataNoiseAsset = strataNoiseAsset;
        chunks.Add(chunkPosition, chunkBehavior);

        return chunkBehavior;
    }

    private void SpawnShop()
    {
        // determine a location
        shopZoneObject.transform.position = GetLocationAboveGround(0);
        shopZoneObject.GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    private Vector3 GetLocationAboveGround(int i)
    {
        if (i > 5)
        {
            Debug.LogError("too many attempts");
            return Vector3.zero;
        }

        var rX = new Random().Next(-25, 41);
        var rZ = new Random().Next(-25, 41);

        var point = new Vector3(rX, 96.01f, rZ);

        RaycastHit hit;
        if (Physics.Raycast(point, Vector3.down, out hit, 0.25f))
        {
            return point;
        } else
        {
            return GetLocationAboveGround(i++);
        }

    }

    private void SpawnEnemies()
    {

    }

    private void MovePlayerToLocation()
    {
        var temp = GetLocationAboveGround(0);
        temp.y = 120;
        playerControllerReference.transform.position = temp;
    }
}