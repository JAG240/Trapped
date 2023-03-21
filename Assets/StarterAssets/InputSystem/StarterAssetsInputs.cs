using UnityEngine;
using Cinemachine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Disable Movements")]
		public bool cameraMovementDisabled = false;
		public bool movementDisabled = false;

		private SceneManager sceneManager;
		private CharacterController characterController;
		private FirstPersonController firstPersonController;
		[SerializeField] private Transform virtualCam;

        private void Start()
        {
			sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
			characterController = GetComponent<CharacterController>();
			firstPersonController = GetComponent<FirstPersonController>();

			sceneManager.enterCar += EnterCar;
			sceneManager.exitCar += ExitCar;

			sceneManager.playerDeath += Die;
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			if (movementDisabled)
				return;

			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook && !cameraMovementDisabled)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			//disabled jump
			//JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnCrouch(InputValue value)
		{
			//1 for crouch pressed 0 for crouch released

			//

			firstPersonController.SetCrouch(value.Get<float>() == 1);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private void EnterCar(Car car)
        {
			firstPersonController.allowMovement = false;
			characterController.enabled = false;
			movementDisabled = true;
			cameraMovementDisabled = true;

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			cursorLocked = false;

			transform.root.position = car.GetSeatPosition();
			firstPersonController.SnapLookAt(car.GetLookPosition());
		}

		private void ExitCar(Car car)
        {
			transform.root.position = car.GetExitPosition();
			characterController.enabled = true;
			firstPersonController.allowMovement = true;
			movementDisabled = false;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			cursorLocked = false;

			cameraMovementDisabled = false;
		}

		private void Die(KillerStateManager killer)
        {
			move = Vector2.zero;
			look = Vector2.zero;

			firstPersonController.allowMovement = false;
			characterController.enabled = false;
			movementDisabled = true;
			cameraMovementDisabled = true;

			firstPersonController.LookAt(killer.transform.position);
        }
	}
	
}