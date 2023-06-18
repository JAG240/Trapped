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

		private bool reading = false;
		private bool paused = false;

        private void Start()
        {
			sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
			characterController = GetComponent<CharacterController>();
			firstPersonController = GetComponent<FirstPersonController>();

			sceneManager.enterCar += EnterCar;
			sceneManager.exitCar += ExitCar;

			sceneManager.readNote += ReadNote;
			sceneManager.exitNote += CloseNote;

			sceneManager.playerDeath += Die;
			sceneManager.resetLevel += ResetLevel;

			sceneManager.pauseGame += PauseGame;
			sceneManager.resumeGame += ResumeGame;

			sceneManager.playerPrefsUpdated += firstPersonController.LoadPlayerPref;
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

		public void OnEscape()
        {
			if (reading)
				sceneManager.CloseNote();
			else if (paused)
				sceneManager.ResumeGame();
			else
				sceneManager.PauseGame();

        }

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnCrouch(InputValue value)
		{
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
		
		public void StopMovement()
        {
			move = Vector2.zero;
			look = Vector2.zero;
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
			DisableCamera();
			transform.root.position = car.GetSeatPosition();
			firstPersonController.SnapLookAt(car.GetLookPosition());
		}

		private void ExitCar(Car car)
		{
			transform.root.position = car.GetExitPosition();
			EnableCamera();
			cameraMovementDisabled = false;
		}

		private void DisableCamera()
        {
			firstPersonController.allowMovement = false;
			characterController.enabled = false;
			movementDisabled = true;
			cameraMovementDisabled = true;

			StopMovement();

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			cursorLocked = false;
		}

		private void EnableCamera()
        {
			characterController.enabled = true;
			firstPersonController.allowMovement = true;
			movementDisabled = false;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			cursorLocked = false;
			cameraMovementDisabled = false;
		}

		private void PauseGame()
        {
			DisableCamera();
			paused = true;
        }

		private void ResumeGame()
        {
			EnableCamera();
			paused = false;
        }

		private void ReadNote()
		{
			DisableCamera();
			reading = true;
		}

		private void CloseNote()
        {
			EnableCamera();
			reading = false;
		}

		private void Die(KillerStateManager killer)
        {
			DisableCamera();
			firstPersonController.LookAt(killer.transform.position);
        }

        private void ResetLevel()
        {
			transform.position = sceneManager.playerRespawnPoint;
			EnableCamera();
			firstPersonController.LookAt(Vector3.right);
		}
    }
	
}