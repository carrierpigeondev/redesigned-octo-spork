using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapons : MonoBehaviour
{
    public List<Weapon> OwnedWeapons;
    public Weapon[] WeaponsInToolbar = { Weapon.None, Weapon.None, Weapon.None };
    public int CurrentWeaponIndex = 0;

    public WeaponsToolbarBehavior ToolbarRef;

    private InputSystem_Actions Inputs;

    private void Awake()
    {
        Inputs = new InputSystem_Actions();
    }

    private void Start()
    {
        ToolbarRef.ToolbarIndicatorSet(CurrentWeaponIndex); // do once on start
    }

    private void OnEnable()
    {
        Inputs.UI.ScrollWheel.Enable();
        Inputs.UI.ScrollWheel.performed += OnScrollWheelPerformed;
    }

    private void OnDisable()
    {
        Inputs.UI.ScrollWheel.performed -= OnScrollWheelPerformed;
        Inputs.UI.ScrollWheel.Disable();
    }

    private void OnScrollWheelPerformed(InputAction.CallbackContext context)
    {
        Vector2 scrollDelta = context.ReadValue<Vector2>();
        if (scrollDelta.y > 0)
        {
            IncrementWeaponIndex();
        } else if (scrollDelta.y < 0)
        {
            DecrementWeaponIndex();
        }
    }

    // LOGIC IS FLATOUT WRONG?
    public void IncrementWeaponIndex()
    {
        int potentialWeaponIndex = CurrentWeaponIndex + 1;
        if (potentialWeaponIndex > WeaponsInToolbar.Length - 1)
        {
            potentialWeaponIndex = 0;
        }
        CurrentWeaponIndex = potentialWeaponIndex;
        ToolbarRef.ToolbarIndicatorSet(CurrentWeaponIndex);
    }

    public void DecrementWeaponIndex()
    {
        int potentialWeaponsIndex = CurrentWeaponIndex - 1;
        if (potentialWeaponsIndex < 0)
        {
            potentialWeaponsIndex = WeaponsInToolbar.Length - 1;
        }
        CurrentWeaponIndex = potentialWeaponsIndex;
        ToolbarRef.ToolbarIndicatorSet(CurrentWeaponIndex);
    }
}
