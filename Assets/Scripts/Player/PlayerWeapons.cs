using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(WeaponsToolbarBehavior))]
public class PlayerWeapons : MonoBehaviour
{
    public List<Weapon> OwnedWeapons;
    public Weapon[] WeaponsInToolbar = { Weapon.None, Weapon.None, Weapon.None };
    public int CurrentWeaponIndex = 0;

    [SerializeField] private WeaponsToolbarBehavior toolbarRef;

    private InputSystem_Actions inputs;

    private void Start()
    {
        toolbarRef.ToolbarIndicatorSet(CurrentWeaponIndex); // do once on start
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
}
