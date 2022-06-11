using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

	public GameObject pauseMenuUI;
	PlayerInput input;
	InputAction menu;

	[SerializeField] private bool isPaused = false;

	void Awake()
	{
		input = new PlayerInput();
	}

	void OnEnable()
	{
		menu = input.Menu.Pause;
		menu.Enable();

		menu.performed += Pause;
	}

	void OnDisable()
	{
		menu.Disable();
	}

	void Pause(InputAction.CallbackContext ctx)
	{
		isPaused = !isPaused;

		if(isPaused)
		{
			Paused();
		}
		else
		{
			Resume();
		}
	}

	 public void Resume()
	{
		Time.timeScale = 1f;
		pauseMenuUI.SetActive(false);
		isPaused = false;
	}

	void Paused()
	{
		Time.timeScale = 0f;
		pauseMenuUI.SetActive(true);
	}

	public void Exit()
	{
		Application.Quit();
	}
}
