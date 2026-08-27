using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsToolbarBehavior : MonoBehaviour
{
    public PlayerWeapons PlayerWeaponsReference;
    public RawImage[] WeaponImageIcons;

    [SerializeField] private Weapon[] weaponsCached = { Weapon.None, Weapon.None, Weapon.None };

    public GameObject[] SelectedIndicatorObjects;

    private void Update()
    {
        Weapon[] currentPlayerWeapons = PlayerWeaponsReference.WeaponsInToolbar;
        if (currentPlayerWeapons.SequenceEqual(weaponsCached)) return;

        Array.Copy(currentPlayerWeapons, weaponsCached, weaponsCached.Length);
        for (int i = 0; i < weaponsCached.Length; i++)
        {
            WeaponImageIcons[i].texture = currentPlayerWeapons[i].GetWeaponToolbarTexture();
        }
    }

    public void ToolbarIndicatorSet(int index)
    {
        for (int i = 0; i < SelectedIndicatorObjects.Length; i++)
        {
            if (i == index)
            {
                SelectedIndicatorObjects[i].SetActive(true);
            } else
            {
                SelectedIndicatorObjects[i].SetActive(false);
            }
        }
    }
}
