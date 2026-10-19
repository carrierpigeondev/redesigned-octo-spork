using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerWeapons))]
public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions inputs;
    
    private CharacterController controller;
    private PlayerWeapons weapons;
    
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpVelocity = 7.0f;
    [SerializeField] private Camera camera;
    [SerializeField] private ResourceUIParentBehaviour ruipb;
    [SerializeField] private ShopBehavior shop;
    [SerializeField] private World w;

    private float pitch = 0.0f;
    private float verticalVelocity = 0.0f;

    private List<Action> updateActions = new List<Action>();

    private void Awake()
    {
        inputs = new InputSystem_Actions();
        
        controller = GetComponent<CharacterController>();
        weapons = GetComponent<PlayerWeapons>();
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Look.performed += OnLook;
        inputs.Player.Jump.performed += OnJump;
        inputs.Player.Jump.performed += AdvanceToStory01;
        inputs.UI.ScrollWheel.performed += OnScrollWheelPerformed;
        inputs.Player.Attack.performed += OnFire;
        inputs.Player.Crouch.performed += OnOpenResourceUI;
        inputs.Player.Interact.performed += OnTeleport;
    }

    private void OnDisable()
    {
        inputs.Player.Interact.performed -= OnTeleport;
        inputs.Player.Crouch.performed -= OnOpenResourceUI;
        inputs.Player.Attack.performed -= OnFire;
        inputs.UI.ScrollWheel.performed -= OnScrollWheelPerformed;
        inputs.Player.Jump.performed -= AdvanceToStory01;
        inputs.Player.Jump.performed -= OnJump;
        inputs.Player.Look.performed -= OnLook;
        inputs.Disable();
    }

    private void Update()
    {
        for (int i = 0; i < updateActions.Count; i++)
        {
            updateActions[i]();
        }
        
        if (shop.ParentObjectMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (transform.position.y < -1000)
        {
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, 500, transform.position.z);
            controller.enabled = true;
        }
    }

    public void AddUpdateActions()
    {
        updateActions.Add(HandleMovement);
        updateActions.Add(CalculateGravity);
    }

    public void RemoveUpdateActions()
    {
        updateActions.Remove(HandleMovement);
        updateActions.Remove(CalculateGravity);
    }

    private void AdvanceToStory01(InputAction.CallbackContext context)
    {
        if (StoryManager.CurrentState == StoryState._00_Beginning)
        {
            StoryManager.CurrentState = StoryState._01_GoToShopZone;
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = inputs.Player.Move.ReadValue<Vector2>();
        Vector3 inputVec = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 moveVec = moveSpeed * inputVec;
        
        controller.Move(new Vector3(moveVec.x, verticalVelocity, moveVec.z) * Time.deltaTime);
    }

    private void CalculateGravity()
    {
        // keep negative so that it sticks to the ground unless explicitly positive
        if (controller.isGrounded && verticalVelocity < 0.0f) verticalVelocity = -2.0f;
        verticalVelocity += gravity * Time.deltaTime;
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

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!controller.isGrounded) return;
        verticalVelocity += jumpVelocity;
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        weapons.FireCurrent(camera.transform.position, camera.transform.forward);
    }

    private void OnOpenResourceUI(InputAction.CallbackContext context)
    {
        // if (ruipb.IsOpen)
        // {
        //     ruipb.MoveOut();
        //     ruipb.IsOpen = false;
        // }
        // else
        // {
        //     ruipb.MoveIn();
        //     ruipb.IsOpen = true;
        // }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
        if (shop.CanBeInteractedWith)
        {
            // if (!ruipb.IsOpen) ruipb.MoveIn();
            shop.ToggleMenu();
        }
    }

    private void OnTeleport(InputAction.CallbackContext context)
    {
        Debug.Log("Teleported");
        w.MovePlayerToLocation();
    }
}
