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

        WeaponFireInformation info = new WeaponFireInformation(10, 2048);
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