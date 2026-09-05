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

    public static void Fire(this Weapon weapon, Vector3 origin, Vector3 direction)
    {
        if (weapon == Weapon.None) return;
        
        Debug.Log($"fired a weapon named {Enum.GetName(typeof(Weapon), weapon)}");

        WeaponFireInformation info = new WeaponFireInformation(100, 2048);
        if (Physics.Raycast(origin, direction, out RaycastHit hit, info.Distance))
        {
            Debug.DrawRay(origin, direction * hit.distance, Color.red);
            Debug.Log("hit " + hit.transform.name);
        }
        else
        {
            Debug.DrawRay(origin, direction * info.Distance, Color.white);
            Debug.Log("hit null");
        }
    }
}

struct WeaponFireInformation
{
    public float Damage;
    public float Distance;
    
    public WeaponFireInformation(float damage, float distance)
    {
        Damage = damage;
        Distance = distance;
    }
}