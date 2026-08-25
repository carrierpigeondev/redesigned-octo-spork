using System.Linq;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public Weapon[] OwnedWeapons;
    public Weapon[] WeaponsInToolbar = { Weapon.None, Weapon.None, Weapon.None };
    public int CurrentWeaponIndex;

    private void Start()
    {

    }

    public void IncrementWeaponIndex()
    {
        int potentialWeaponIndex = CurrentWeaponIndex + 1;
        if (potentialWeaponIndex > OwnedWeapons.Length - 1)
        {
            potentialWeaponIndex = 0;
        }
        CurrentWeaponIndex = potentialWeaponIndex;
    }

    public void DecrementWeaponIndex()
    {
        int potentialWeaponsIndex = CurrentWeaponIndex - 1;
        if (potentialWeaponsIndex < 0)
        {
            potentialWeaponsIndex = OwnedWeapons.Length - 1;
        }
        CurrentWeaponIndex = potentialWeaponsIndex;
    }
}
