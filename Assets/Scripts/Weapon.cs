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
        Debug.Log($"getting weapon for the type: {weapon}");
        return UnityResourceLoader.WeaponToolbarTextures[(int)weapon];
    }
}