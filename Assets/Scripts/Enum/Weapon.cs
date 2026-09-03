using System;
using UnityEngine;
using UnityEngine.UI;

public enum Weapon
{
    None,
    Pistol,
    Rifle,
    AdvancedPistol,
    AdvancedRifle
}

public static class WeaponExtensions
{
    public static Texture2D GetWeaponToolbarTexture(this Weapon weapon)
    {
        return UnityResourceLoader.WeaponToolbarTextures[(int)weapon];
    }

    public static void Fire(this Weapon weapon)
    {
        Debug.Log($"fired a weapon named {Enum.GetName(typeof(Weapon), weapon)}");
    }
}

struct WeaponFireInformation
{
    float damage;
}