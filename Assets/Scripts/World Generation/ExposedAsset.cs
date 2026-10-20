using System;
using UnityEngine;
using Random = System.Random;

public class ExposedAsset : MonoBehaviour
{
    private void Awake()
    {
        int seed = Noise.ExposeSeed();
        Random r = new Random(seed);
        WeaponExtensions.Offset = r.Next(0, 1000);

        gunSource = gs;
        hitSource = hs;
    }

    public FastNoiseAsset Noise;

    [SerializeField] private AudioSource gs;
    [SerializeField] private AudioSource hs;
    
    public static AudioSource gunSource;
    public static AudioSource hitSource;
}
