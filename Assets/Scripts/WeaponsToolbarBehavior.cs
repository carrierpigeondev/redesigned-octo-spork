using UnityEngine;
using UnityEngine.UI;

public class WeaponsToolbarBehavior : MonoBehaviour
{
    public PlayerWeapons PlayerWeaponsReference;
    public RawImage[] WeaponImageIcons;


    private Weapon[] weaponsCached;

    private void Update()
    {
        Weapon[] currentPlayerWeapons = PlayerWeaponsReference.WeaponsInToolbar;
        //if (currentPlayerWeapons == weaponsCached) return;

        weaponsCached = currentPlayerWeapons;
        for (int i = 0; i < weaponsCached.Length; i++)
        {
            WeaponImageIcons[i].texture = currentPlayerWeapons[i].GetWeaponToolbarTexture();
        }
    }
}
