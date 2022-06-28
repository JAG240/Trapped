using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float walkCollisionPower;
    [SerializeField] private float runCollisionPower;
    private float collisionPower;
    private float movementSpeed;
    private PlayerControls playerControls;
    private CharacterController characterController;
    private Vector3 characterVelocity;
    private float gravity;
    private bool isCrouched = false;
    private bool releaseCrouch = false;
    private Transform eyes;
    private bool useGravity = true;

    private void Start()
    {
        playerControls = GameObject.Find("InputManager").GetComponent<InputManager>().playerControls;
        characterController = GetComponent<CharacterController>();
        characterVelocity = characterController.velocity;
        gravity = Physics.gravity.y;
        eyes = transform.Find("Eyes");
        collisionPower = walkCollisionPower;

        playerControls.Basic.Run.performed += (context) => Run(true);
        playerControls.Basic.Run.canceled += (context) => Run(false);

        playerControls.Basic.Crouch.performed += (context) => Crouch();
        playerControls.Basic.Crouch.canceled += (context) => Uncrouch();

        playerControls.Basic.Jump.performed += (context) => Jump();

        movementSpeed = walkSpeed;
    }

    private void OnDisable()
    {
        useGravity = false;
    }

    private void OnEnable()
    {
        useGravity = true;
    }

    private void Update()
    {
        if(useGravity)
            ApplyGravity();

        if (playerControls.Basic.Walk.IsPressed())
            Walk(playerControls.Basic.Walk.ReadValue<Vector2>());

        if (releaseCrouch)
            Uncrouch();
    }

    private void ApplyGravity()
    {
        characterVelocity.y += gravity * Time.deltaTime;

        if (characterController.isGrounded && characterVelocity.y < -1.0)
            characterVelocity.y = -0.5f;

        characterController.Move(characterVelocity * Time.deltaTime);
    }

    private void Run(bool state)
    {
        if (isCrouched)
            return;

        if (state)
        {
            movementSpeed = runSpeed;
            collisionPower = runCollisionPower;
        }
        else
        {
            movementSpeed = walkSpeed;
            collisionPower = walkCollisionPower;
        }
    }

    private void Walk(Vector2 input)
    {
        Vector3 movement = input.x * transform.right + input.y * transform.forward;
        characterController.Move(movement * Time.deltaTime * movementSpeed);
    }

    private void Jump()
    {
        if (characterController.isGrounded)
            characterVelocity.y += Mathf.Sqrt(jumpHeight * -gravity);
    }

    private void Crouch()
    {
        releaseCrouch = false;
        isCrouched = true;
        characterController.height /= 3;
        characterController.center = new Vector3(0, -1f, 0);
        movementSpeed = crouchSpeed;
        eyes.localPosition = new Vector3(0f, -0.32f, 0.45f);
    }

    private void Uncrouch()
    {
        if (!releaseCrouch)
            releaseCrouch = true;

        if (Physics.Raycast(transform.position + new Vector3(0f, -0.2f, 0f), transform.up, 2.2f))
            return;

        releaseCrouch = false;
        isCrouched = false;
        characterController.height = 3;
        characterController.center = Vector3.zero;
        movementSpeed = walkSpeed;
        eyes.localPosition = new Vector3(0f, 1f, 0.45f);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (!body || body.isKinematic)
            return;

        Vector3 pushDir = hit.transform.position - transform.position;
        pushDir.y = 0.2f;
        body.AddForce(pushDir * collisionPower, ForceMode.Impulse);
    }
}
