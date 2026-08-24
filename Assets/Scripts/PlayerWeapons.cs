using System.Linq;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public Weapon[] OwnedWeapons;
    public Weapon[] WeaponToolbar;
    public int CurrentWeaponIndex;

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
