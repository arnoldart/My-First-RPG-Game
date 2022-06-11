using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacter : MonoBehaviour
{

	PlayerInput playerInput;
	CharacterController characterController;
	Animator animator;

	int isRunningHash;
	int isSlowRunHash;

	Vector2 currentMovementInput;
	Vector3 currentMovement;
	Vector3 currentRunMovement;
	bool isMovementPressed;
	bool isRunPressed;
	float rotationFactorPerFrame = 15.0f;
	float runMultiplier = 3.0f;

	void Awake()
	{
		playerInput = new PlayerInput();
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();

		isRunningHash = Animator.StringToHash("isRunning");
		isSlowRunHash = Animator.StringToHash("isSlowRun");

		playerInput.CharacterController.Movement.started += onMovementInput;
		playerInput.CharacterController.Movement.canceled += onMovementInput;
		playerInput.CharacterController.Movement.performed += onMovementInput;

		playerInput.CharacterController.Run.started += onRun;
		playerInput.CharacterController.Run.canceled += onRun;
	} 

	void onMovementInput(InputAction.CallbackContext ctx)
	{
		currentMovementInput = ctx.ReadValue<Vector2>();
		currentMovement.x = currentMovementInput.x;
		currentMovement.z = currentMovementInput.y;
		currentRunMovement.x = currentMovementInput.x * runMultiplier;
		currentRunMovement.z = currentMovementInput.y * runMultiplier;

		isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
	}

	void onRun(InputAction.CallbackContext ctx)
	{
		isRunPressed = ctx.ReadValueAsButton();
	}

	void handleRotation()
	{
		Vector3 postitionToLookAt;

		postitionToLookAt.x = currentMovement.x;
		postitionToLookAt.y = 0.0f;
		postitionToLookAt.z = currentMovement.z;

		Quaternion currentRotation = transform.rotation;

		if (isMovementPressed)
		{
			Quaternion targetRotation = Quaternion.LookRotation(postitionToLookAt);
			transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
		}
	}

	void handleAnimation()
	{
		bool isSlowRunning = animator.GetBool(isSlowRunHash);
		bool isRunning = animator.GetBool(isRunningHash);

		if (isMovementPressed && !isSlowRunning)
		{
			animator.SetBool(isSlowRunHash, true);
		}
		else if (!isMovementPressed && isSlowRunning)
		{
			animator.SetBool(isSlowRunHash, false);
		}

		if ((isMovementPressed && isRunPressed) && !isRunning)
		{
			animator.SetBool(isRunningHash, true);
		}
		else if ((!isMovementPressed || !isRunPressed) && isRunning)
		{
			animator.SetBool(isRunningHash, false);
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		handleRotation();
		handleAnimation();

		if (isRunPressed)
		{
			characterController.Move(currentRunMovement * Time.deltaTime);
		}
		else
		{
			characterController.Move(currentMovement * Time.deltaTime);
		}

	}

	void OnEnable()
	{
		playerInput.CharacterController.Enable();
	}

	void OnDisable()
	{
		playerInput.CharacterController.Disable();
	}
}
