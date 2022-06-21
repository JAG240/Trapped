using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float horizontalSpeed;
    private PlayerControls playerControls;
    private Transform player;
    private float verticalDelta = 0f;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerControls.Basic.Look.performed += Look;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerControls.Basic.Look.performed -= Look;
    }

    void Awake()
    {
        playerControls = GameObject.Find("InputManager").GetComponent<InputManager>().playerControls;
        player = transform.parent;
    }

    public void EnableCameraMovement(bool state)
    {
        if(state)
            playerControls.Basic.Look.performed += Look;
        else
            playerControls.Basic.Look.performed -= Look;
    }

    private void Look(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();

        verticalDelta -= delta.y * Time.deltaTime * verticalSpeed;
        verticalDelta = Mathf.Clamp(verticalDelta, -90f, 90f);
        transform.localEulerAngles = new Vector3(verticalDelta, 0f, 0f);

        player.Rotate(Vector3.up * delta.x * horizontalSpeed);
    }
}
