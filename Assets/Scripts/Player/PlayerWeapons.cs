using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerWeapons : MonoBehaviour
{
    public List<Weapon> OwnedWeapons;
    public Weapon[] WeaponsInToolbar = { Weapon.None, Weapon.None };
    public int CurrentWeaponIndex = 0;

    [SerializeField] private WeaponsToolbarBehavior toolbarRef;

    private void Start()
    {
        toolbarRef.ToolbarIndicatorSet(CurrentWeaponIndex); // do once on start
    }

    public void UpdateToolbar()
    {
        UpdateToolbarPistol();
        UpdateToolbarRifle();
    }

    private void UpdateToolbarPistol()
    {
        if (OwnedWeapons.Contains(Weapon.Pistol))
        {
            WeaponsInToolbar[0] = Weapon.Pistol;
        }

        if (OwnedWeapons.Contains(Weapon.AdvancedPistol))
        {
            WeaponsInToolbar[0] = Weapon.AdvancedPistol;
        }
    }

    private void UpdateToolbarRifle()
    {
        if (OwnedWeapons.Contains(Weapon.Rifle))
        {
            WeaponsInToolbar[1] = Weapon.Rifle;
        }

        if (OwnedWeapons.Contains(Weapon.AdvancedRifle))
        {
            WeaponsInToolbar[1] = Weapon.AdvancedRifle;
        }
    }

    public void IncrementWeaponIndex()
    {
        int potentialWeaponIndex = CurrentWeaponIndex + 1;
        if (potentialWeaponIndex > WeaponsInToolbar.Length - 1)
        {
            potentialWeaponIndex = 0;
        }
        CurrentWeaponIndex = potentialWeaponIndex;
        toolbarRef.ToolbarIndicatorSet(CurrentWeaponIndex);
    }

    public void DecrementWeaponIndex()
    {
        int potentialWeaponsIndex = CurrentWeaponIndex - 1;
        if (potentialWeaponsIndex < 0)
        {
            potentialWeaponsIndex = WeaponsInToolbar.Length - 1;
        }
        CurrentWeaponIndex = potentialWeaponsIndex;
        toolbarRef.ToolbarIndicatorSet(CurrentWeaponIndex);
    }

    public void FireCurrent(Vector3 origin, Vector3 direction)
    {
        WeaponsInToolbar[CurrentWeaponIndex].Fire(origin, direction);
    }
}
