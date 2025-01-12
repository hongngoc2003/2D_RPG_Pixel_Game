using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 moveInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool dashInput { get; private set; }
    public bool crystalInput { get; private set; }
    public bool parryInput { get; private set; }
    public bool blackholeInput { get; private set; }
    public bool attackInput { get; private set; }
    public bool aimInput { get; private set; }
    public bool flaskInput { get; private set; }

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction crystalAction;
    private InputAction parryAction;
    private InputAction blackholeAction;
    private InputAction attackAction;
    private InputAction aimAction;
    private InputAction flaskAction;




    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Hủy object mới nếu đã có instance
        } else {
            instance = this; // Gán instance mới
            DontDestroyOnLoad(gameObject); // Đảm bảo instance tồn tại xuyên scene
        }

        playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
    }
    private void Update() {
        UpdateInput();
    }
    private void SetupInputActions() {
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        dashAction = playerInput.actions["Dash"];
        crystalAction = playerInput.actions["Crystal"];
        blackholeAction = playerInput.actions["Blackhole"];
        parryAction = playerInput.actions["Parry"];
        aimAction = playerInput.actions["Aim"];
        flaskAction = playerInput.actions["Flask"];

    }

    private void UpdateInput() {
        moveInput = moveAction.ReadValue<Vector2>();
        jumpInput = jumpAction.WasPressedThisFrame();
        dashInput = dashAction.WasPressedThisFrame();
        parryInput = parryAction.WasPressedThisFrame();
        crystalInput = crystalAction.WasPressedThisFrame();
        attackInput = attackAction.WasPressedThisFrame();
        blackholeInput = blackholeAction.WasPressedThisFrame();
        aimInput = aimAction.IsPressed();
        flaskInput = flaskAction.WasPressedThisFrame(); 
    }
}
