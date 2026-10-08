using System;
using UnityEngine;
using Unity.Mathematics;
using Random = System.Random;

[CreateAssetMenu(menuName =  "Noise/Fast Noise Asset", fileName = "Fast Noise Asset")]
public class FastNoiseAsset : ScriptableObject
{
    [Header("General Settings")]
    [SerializeField] private FastNoiseLite.NoiseType noiseType = FastNoiseLite.NoiseType.OpenSimplex2;
    [SerializeField] private FastNoiseLite.RotationType3D rotationType3D = FastNoiseLite.RotationType3D.None;
    [SerializeField] private int seed = 082826; // today :D -- just a placeholder, RandomizeSeed randomizes it
    [SerializeField] private float frequency = 0.01f;
    
    [Header("Fractal")]
    [SerializeField] private FastNoiseLite.FractalType fractalType = FastNoiseLite.FractalType.None;
    [SerializeField] private int octaves = 3;
    [SerializeField] private float lacunarity = 2.0f;
    [SerializeField] private float gain = 0.5f;
    [SerializeField] private float weightedStrength = 0.0f;
    [SerializeField] private float pingPongStrength = 2.0f;

    [Header("Cellular")]
    [SerializeField] private FastNoiseLite.CellularDistanceFunction cellularDistanceFunction =
        FastNoiseLite.CellularDistanceFunction.EuclideanSq;
    [SerializeField] private FastNoiseLite.CellularReturnType cellularReturnType =
        FastNoiseLite.CellularReturnType.Distance;
    [SerializeField] private float cellularJitter = 1.0f;

    [Header("Domain Warp")]
    [SerializeField] private FastNoiseLite.DomainWarpType domainWarpType = FastNoiseLite.DomainWarpType.OpenSimplex2;
    [SerializeField] private float domainWarpAmp = 1.0f;
    
    [Header("Domain Warp Fractal??")]
    
    private FastNoiseLite fastNoiseInstance;

    public int ExposeSeed()
    {
        return seed;
    }

    private void OnEnable()
    {
        fastNoiseInstance = new FastNoiseLite();
        RandomizeSeed();
        SetNoiseSettings(ref fastNoiseInstance);
    }

    public FastNoiseLite GetNoiseStruct()
    {
        FastNoiseLite noise = new FastNoiseLite();
        SetNoiseSettings(ref noise);
        return noise;
    }

    private void RandomizeSeed()
    {
        // pseudorandomness is ok here since it doesnt need to be secure
        Random random = new Random();
        seed = random.Next();
    }

    public void SetNoiseSettings(ref FastNoiseLite instance)
    {
        instance.SetNoiseType(noiseType);
        instance.SetRotationType3D(rotationType3D);
        instance.SetSeed(seed);
        instance.SetFrequency(frequency);
        
        instance.SetFractalType(fractalType);
        instance.SetFractalOctaves(octaves);
        instance.SetFractalLacunarity(lacunarity);
        instance.SetFractalGain(gain);
        instance.SetFractalWeightedStrength(weightedStrength);
        instance.SetFractalPingPongStrength(pingPongStrength);
        
        instance.SetCellularDistanceFunction(cellularDistanceFunction);
        instance.SetCellularReturnType(cellularReturnType);
        instance.SetCellularJitter(cellularJitter);
        
        instance.SetDomainWarpType(domainWarpType);
        instance.SetDomainWarpAmp(domainWarpAmp);
    }
    
    /// <summary>
    /// 2D noise at given position using current settings
    /// </summary>
    /// <param name="minHeight"></param>
    /// <param name="maxHeight"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>
    /// Noise output bounded between minHeight...maxHeight
    /// </returns>
    public float GetNoiseClampedHeight(float minHeight, float maxHeight, float x, float y)
    {
        fastNoiseInstance.DomainWarp(ref x, ref y);
        return // floorHeight to maxHeight
        (
            (
                (
                    fastNoiseInstance.GetNoise(x, y)    // range -1           to 1
                    + 1                     // range  0           to 2
                ) / 2                       // range  0           to 1
            ) * (maxHeight - minHeight)     // range  0           to (maxHeight - minHeight)
        ) + minHeight;                      /* range  minHeight to ((maxHeight - minHeight) + minHeight)
                                             * simplifies ===> minHeight to maxHeight
                                             */
    }

    /// <summary>
    /// 2D noise at given position using current settings
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>
    /// Noise output bounded between 0...1
    /// </returns>
    public float GetNoiseBounded01(float x, float y)
    {
        fastNoiseInstance.DomainWarp(ref x, ref y);
        return (fastNoiseInstance.GetNoise(x, y) + 1) / 2;
    }

    /// <summary>
    /// 3D noise at given position using current settings
    /// </summary>
    /// <returns>
    /// Noise output bounded between -1...1
    /// </returns>
    public float GetNoise(float x, float y, float z)
    {
        fastNoiseInstance.DomainWarp(ref x, ref y, ref z);
        return fastNoiseInstance.GetNoise(x, y, z);
    }

    /// <summary>
    /// 3D noise at given position using current settings
    /// </summary>
    /// <returns>
    /// Noise output bounded between -1...1
    /// </returns>
    public float GetNoise(float3 position)
    {
        fastNoiseInstance.DomainWarp(ref position.x, ref position.y, ref position.z);
        return fastNoiseInstance.GetNoise(position.x, position.y, position.z);
    }

    /// <summary>
    /// 3D noise at given position using current settings
    /// </summary>
    /// <returns>
    /// Noise output bounded between 0...1
    /// </returns>
    public float GetNoiseBounded01(float3 position)
    {
        fastNoiseInstance.DomainWarp(ref position.x, ref position.y, ref position.z);
        return (fastNoiseInstance.GetNoise(position.x, position.y, position.z) + 1) / 2;
    }

    /// <summary>
    /// 2D noise at given position using current settings
    /// </summary>
    /// <returns>
    /// Noise output bounded between -1...1
    /// </returns>
    public float GetNoise(float x, float y)
    {
        fastNoiseInstance.DomainWarp(ref x, ref y);
        return fastNoiseInstance.GetNoise(x, y);
    }
}