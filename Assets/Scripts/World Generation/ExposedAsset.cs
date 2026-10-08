using System;
using UnityEngine;
using Random = System.Random;

public class ExposedAsset : MonoBehaviour
{
    private void Start()
    {
        int seed = Noise.ExposeSeed();
        Random r = new Random(seed);
        WeaponExtensions.Offset = r.Next(0, 1000);
    }

    public FastNoiseAsset Noise;
}
