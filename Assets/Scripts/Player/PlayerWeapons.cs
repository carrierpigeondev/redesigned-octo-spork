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

    private InputSystem_Actions inputs;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
    }

    private void Start()
    {
        ToolbarRef.ToolbarIndicatorSet(CurrentWeaponIndex); // do once on start
    }

    private void OnEnable()
    {
        inputs.UI.ScrollWheel.Enable();
        inputs.UI.ScrollWheel.performed += OnScrollWheelPerformed;
    }

    private void OnDisable()
    {
        inputs.UI.ScrollWheel.performed -= OnScrollWheelPerformed;
        inputs.UI.ScrollWheel.Disable();
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
