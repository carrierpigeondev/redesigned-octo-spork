using System;
using System.Collections.Generic;
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
    public static float Offset;
    
    public static Texture2D GetWeaponToolbarTexture(this Weapon weapon)
    {
        return UnityResourceLoader.WeaponToolbarTextures[(int)weapon];
    }

    public static void Fire(this Weapon weapon, Vector3 origin, Vector3 direction)
    {
        if (weapon == Weapon.None) return;
        
        Debug.Log($"fired a weapon named {Enum.GetName(typeof(Weapon), weapon)}");

        WeaponFireInformation info = weapon.CreateFireInformation();
        if (Physics.Raycast(origin, direction, out RaycastHit hit, info.Distance))
        {
            Debug.Log("hit " + hit.transform.name);

            BaseEnemyBehaviour enemyBehaviour = hit.transform.GetComponentInParent<BaseEnemyBehaviour>();
            if (enemyBehaviour) {
                InteractWithEnemy(enemyBehaviour, info);
            }
        }
    }

    private static void InteractWithEnemy(BaseEnemyBehaviour enemyBehaviour, WeaponFireInformation info)
    {
        enemyBehaviour.TakeDamage(info.Damage);
    }

    private static WeaponFireInformation CreateFireInformation(this Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.Pistol:         return new WeaponFireInformation(10, 2048);
            case Weapon.Rifle:          return new WeaponFireInformation(20, 2048);
            case Weapon.AdvancedPistol: return new WeaponFireInformation(15, 2048);
            case Weapon.AdvancedRifle:  return new WeaponFireInformation(30, 2048);
            default:
                throw new NotImplementedException();
        }
    }

    public static WeaponPurchaseInformation CreatePurchaseInformation(this Weapon weapon)
    {
        if (weapon == Weapon.Pistol)
        {
            Dictionary<Resource, int> _resourceDictionary = new Dictionary<Resource, int>();
            _resourceDictionary.Add(Resource.Ruby, 0);
            _resourceDictionary.Add(Resource.Flesh, 0);
            return new WeaponPurchaseInformation(_resourceDictionary, false);
        }
        
        int baseResources = 30;
        float variation = (Mathf.Sin(Offset) + 1f) / 2f;
        int rubyAmount = Mathf.RoundToInt(baseResources * (int)weapon * (0.5f + variation));
        int fleshAmount = Mathf.RoundToInt(baseResources * (int)weapon * (1.5f - variation));
        
        Dictionary<Resource, int> resourceDictionary = new Dictionary<Resource, int>();
        resourceDictionary.Add(Resource.Ruby, rubyAmount);
        resourceDictionary.Add(Resource.Flesh, fleshAmount);

        if (weapon == Weapon.AdvancedPistol || weapon == Weapon.AdvancedRifle)
        {
            return new WeaponPurchaseInformation(resourceDictionary, true);
        }
        else
        {
            return new WeaponPurchaseInformation(resourceDictionary, false);
        }
    }
}

struct WeaponFireInformation
{
    public int Damage;
    public float Distance;
    
    public WeaponFireInformation(int damage, float distance)
    {
        Damage = damage;
        Distance = distance;
    }
}

public struct WeaponPurchaseInformation
{
    public Dictionary<Resource, int> ResourceDictionary;
    public bool IsAdvanced;

    public WeaponPurchaseInformation(Dictionary<Resource, int> resourceDictionary, bool isAdvanced)
    {
        ResourceDictionary = resourceDictionary;
        IsAdvanced = isAdvanced;
    }
    
}