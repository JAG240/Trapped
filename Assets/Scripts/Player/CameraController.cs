using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private bool lookDisabled = false;

    private PlayerControls playerControls;
    private Transform player;
    private float verticalDelta = 0f;
    private SceneManager sceneManager;

    void Awake()
    {
        playerControls = GameObject.Find("InputManager").GetComponent<InputManager>().playerControls;
        player = transform.parent;
        playerControls.Basic.Look.performed += Look;
    }

    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        sceneManager.playerDeath += (context) => EnableCameraMovement(false);
        sceneManager.resetLevel += () => EnableCameraMovement(true);

        sceneManager.enterCar += EnterCar;
        sceneManager.exitCar += ExitCar;
    }

    private void EnterCar(Car car)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        lookDisabled = true;
    }

    private void ExitCar(Car car)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lookDisabled = false;
    }

    public void EnableCameraMovement(bool state)
    {
        lookDisabled = !state;
    }

    private void Look(InputAction.CallbackContext context)
    {
        if (lookDisabled)
            return;

        Vector2 delta = context.ReadValue<Vector2>();

        verticalDelta -= delta.y * Time.deltaTime * verticalSpeed;
        verticalDelta = Mathf.Clamp(verticalDelta, -90f, 90f);
        transform.localRotation = Quaternion.Euler(verticalDelta, 0f, 0f);

        player.Rotate(Vector3.up * delta.x * horizontalSpeed);
    }
}
