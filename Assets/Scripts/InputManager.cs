#define USE_NEW_INPUT_SYSTEM

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInputActions playerInputActions;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one InputManager in the scene");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool IsMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    public int GetChangeSelectedUnitAxisThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        int axis = (int)playerInputActions.Player.ChangeSelectedUnit.ReadValue<float>();
        bool pressedThisFrame = playerInputActions.Player.ChangeSelectedUnit.WasPressedThisFrame();
        if (pressedThisFrame)
        {
            return axis;
        }

        return 0;
#else
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return -1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return +1;
        }
#endif
    }

    public bool IsFocusUnitBtnPressed()
    {
#if USE_NEW_INPUT_SYSTEM
        bool pressedThisFrame = playerInputActions.Player.FocusOnUnit.IsPressed();
        return pressedThisFrame;
#else
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return -1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return +1;
        }
#endif
    }

    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        Vector2 inputMoveDir = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        return inputMoveDir;
#endif
    }

    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        float rotateAmount = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotateAmount = -1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotateAmount = +1f;
        }
        
        return rotateAmount;
#endif
    }

    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        float zoomAmount = 0f;
        if (Input.mouseScrollDelta.y > 0f)
        {
            zoomAmount = -1f;
        }
        else if (Input.mouseScrollDelta.y < 0f)
        {
            zoomAmount = +1f;
        }

        return zoomAmount;
#endif
    }

    public bool IsPauseButtonPressedThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.PauseGame.WasPressedThisFrame();
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }

    public int GetKeySelectedActionIndex()
    {
#if USE_NEW_INPUT_SYSTEM
        if (playerInputActions.Player.SelectActionKey.WasPressedThisFrame())
        {
            return (int)playerInputActions.Player.SelectActionKey.ReadValue<float>();
        }

        return -1;
#else
        return -1;
#endif
    }

    public bool IsTestActionPressedThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.TestAction.WasPressedThisFrame();
#else
        return Input.GetKeyDown(KeyCode.T);
#endif
    }
    
}
