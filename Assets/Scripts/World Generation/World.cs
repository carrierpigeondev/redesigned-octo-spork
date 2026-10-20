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
    [SerializeField] private GameObject enemyPrefab;

    private Dictionary<Vector3Int, ChunkBehavior> chunks = new Dictionary<Vector3Int, ChunkBehavior>();

    private async void Start()
    {
        Material blockMaterial = textureManager.BlockMaterial;
        await GenerateWorldAsync(blockMaterial);

        // Move the player
        MovePlayerToLocation();

        // Spawn the shop
        SpawnShop();

        while (StoryManager.CurrentState == StoryState._00_Beginning)
        {
            await Task.Yield();
        } // delay until not beginning

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
        return GetLocationAboveGroundBelowCeilingAtLevel(i, 96.01f, 0.25f, 1000f);
    }

    private Vector3 GetLocationAboveGroundBelowCeilingAtLevel(int i, float level, float groundRange, float ceilingRange)
    {
        if (i > 50)
        {
            Debug.LogError("too many attempts");
            return Vector3.zero;
        }
        
        // hardcoded bounds
        var rX = new Random().Next(-25, 41);
        var rZ = new Random().Next(-25, 41);
        
        var point = new Vector3(rX, level, rZ);

        if (!Physics.Raycast(point, Vector3.down, groundRange))
        {
            // try again because did not hit ground in less than ground range
            return GetLocationAboveGroundBelowCeilingAtLevel(i + 1, level, groundRange, ceilingRange);
        }

        if (Physics.Raycast(point, Vector3.up, ceilingRange))
        {
            // try again because it did hit the ceiling in less than ceiling range
            return GetLocationAboveGroundBelowCeilingAtLevel(i + 1, level, groundRange, ceilingRange);
        }

        return point; // point found is valid
    }

    public void SpawnEnemies(int minCount, int maxCount)
    {
        var count = new Random().Next(minCount, maxCount);
        Debug.Log($"Spawning {count} enemies");

        for (int i = 0; i < count; i++)
        {
            var rY = new Random().Next(0, 60);
            Vector3 position = GetLocationAboveGroundBelowCeilingAtLevel(0, rY + 5f, 10.0f, 7.0f);
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }

    public void MovePlayerToLocation()
    {
        Vector3 position = GetLocationAboveGround(0);
        position.y = 120;

        CharacterController controller =
            playerControllerReference.GetComponent<CharacterController>();

        controller.enabled = false;
        playerControllerReference.transform.position = position;
        controller.enabled = true;
    }
}