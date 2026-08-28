using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerWeapons))]
public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions inputs;
    
    private CharacterController controller;
    private PlayerWeapons weapons;
    
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private Camera camera;

    private float pitch = 0.0f;
    
    private void Awake()
    {
        inputs = new InputSystem_Actions();
        
        controller = GetComponent<CharacterController>();
        weapons = GetComponent<PlayerWeapons>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Look.performed += OnLook;
        inputs.UI.ScrollWheel.performed += OnScrollWheelPerformed;
    }

    private void OnDisable()
    {
        inputs.UI.ScrollWheel.performed -= OnScrollWheelPerformed;
        inputs.Player.Look.performed -= OnLook;
        inputs.Disable();
    }

    private void Update()
    {
        HandleMovement(); // doesn't work as a discrete call on .performed
    }
    
    private void HandleMovement()
    {
        Vector2 moveInput = inputs.Player.Move.ReadValue<Vector2>();
        Vector3 moveVec = transform.right * moveInput.x + transform.forward * moveInput.y;
        moveVec.y = 0.0f;
        
        controller.Move(moveSpeed * Time.deltaTime * moveVec);
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        Vector2 mouseVec = lookInput * sensitivity;

        pitch -= mouseVec.y;
        pitch = Mathf.Clamp(pitch, -89.0f, 89.0f);

        transform.Rotate(Vector3.up * mouseVec.x);
        camera.transform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);
    }
    
    private void OnScrollWheelPerformed(InputAction.CallbackContext context)
    {
        Vector2 scrollDelta = context.ReadValue<Vector2>();
        if (scrollDelta.y > 0) weapons.IncrementWeaponIndex();
        else if (scrollDelta.y < 0) weapons.DecrementWeaponIndex();
    }
}
