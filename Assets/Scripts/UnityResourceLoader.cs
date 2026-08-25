using UnityEngine;

public class UnityResourceLoader : MonoBehaviour
{
    // organizing by what specific behaviours will need to access
    // e.g., WeaponsToolbarBehaviour doesn't need anything other than WeaponToolbarTextures
    public static Texture2D[] WeaponToolbarTextures;

    void Awake()
    {
        LoadWeaponToolbarTextures();
    }

    void LoadWeaponToolbarTextures()
    {
        WeaponToolbarTextures[0] = Resources.Load<Texture2D>("Weapons/Toolbar/None");
        WeaponToolbarTextures[1] = Resources.Load<Texture2D>("Weapons/Toolbar/Pistol");
        WeaponToolbarTextures[2] = Resources.Load<Texture2D>("Weapons/Toolbar/Rifle");
        WeaponToolbarTextures[3] = Resources.Load<Texture2D>("Weapons/Toolbar/AdvancedPistol");
        WeaponToolbarTextures[4] = Resources.Load<Texture2D>("Weapons/Toolbar/AdvancedRifle");
        Debug.Log("Loaded all resources");
    }
}
