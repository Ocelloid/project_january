using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public class OnMoveByMouseEventArgs : EventArgs {
        public UnityEngine.InputSystem.InputAction.CallbackContext context;
    }
    public event EventHandler<OnMoveByMouseEventArgs> OnMoveByMouseAction;
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.MoveByMouse.performed += MoveByMouse_performed;
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }
    private void MoveByMouse_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        OnMoveByMouseAction?.Invoke(this, new OnMoveByMouseEventArgs {
            context = context
        });
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetNormalizedMovementVector() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        // Vector2 inputVector = new Vector2(0, 0);

        // if (Input.GetKey(KeyCode.W)) {
        //     inputVector.y += 1;
        // }
        // if (Input.GetKey(KeyCode.S)) {
        //     inputVector.y -= 1;
        // }
        // if (Input.GetKey(KeyCode.A)) {
        //     inputVector.x -= 1;
        // }
        // if (Input.GetKey(KeyCode.D)) {
        //     inputVector.x += 1;
        // }
        return inputVector.normalized;
    }
}
