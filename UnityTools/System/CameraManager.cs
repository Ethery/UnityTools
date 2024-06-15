using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityTools.Systems.Inputs;

public class CameraManager : Singleton<CameraManager>
{
	private void OnEnable()
	{
		RegisterInputs(true);
	}

	private void RegisterInputs(bool register)
	{
		InputManager.RegisterInput(m_cameraMoveInput
									, new InputManager.InputEvent(OnCameraMovement, InputActionPhase.Performed)
									, register);
		InputManager.RegisterInput(m_cameraMoveInput
									, new InputManager.InputEvent(OnCameraMovementEnd, InputActionPhase.Canceled)
									, register);

		InputManager.RegisterInput(m_cameraSpeedUpInput
									, new InputManager.InputEvent(OnCameraSpeedUp, InputActionPhase.Performed)
									, register);
		InputManager.RegisterInput(m_cameraSpeedUpInput
									, new InputManager.InputEvent(OnCameraSpeedUp, InputActionPhase.Canceled)
									, register);

		InputManager.RegisterInput(m_cameraSpeedControlInput
									, new InputManager.InputEvent(OnCameraSpeedControl, InputActionPhase.Performed)
									, register);
	}


	public void OnCameraMovement(InputAction action)
	{
		wantedMovement = action.ReadValue<Vector2>();
	}
	public void OnCameraMovementEnd(InputAction action)
	{
		wantedMovement = Vector2.zero;
	}

	public void OnCameraSpeedUp(InputAction action)
	{
		speedUp = action.ReadValue<float>() > 0f;
	}

	public void OnCameraSpeedControl(InputAction action)
	{
		float actionValue = action.ReadValue<float>();
		actionValue = Mathf.Clamp(actionValue, -0.2f, 0.2f);
		m_cameraBaseSpeed += actionValue;
		m_cameraBaseSpeed = Mathf.Clamp(m_cameraBaseSpeed, m_cameraMinSpeed, m_cameraMaxSpeed);
	}

	private void FixedUpdate()
	{
		if(wantedMovement != Vector2.zero)
		{
			Vector3 newPosition = transform.position;

			newPosition.x += wantedMovement.x * m_cameraBaseSpeed * Time.deltaTime * (speedUp ? m_cameraSpeedMultiplier : 1);
			newPosition.z += wantedMovement.y * m_cameraBaseSpeed * Time.deltaTime * (speedUp ? m_cameraSpeedMultiplier : 1);

			transform.position = newPosition;
		}
	}

	private void OnDisable()
	{
		RegisterInputs(false);
	}

	private Vector2 wantedMovement = Vector2.zero;
	private bool speedUp = false;

	[SerializeField]
	private InputManager.Input m_cameraMoveInput;
	[SerializeField]
	private InputManager.Input m_cameraSpeedUpInput;
	[SerializeField]
	private InputManager.Input m_cameraSpeedControlInput;

	[SerializeField]
	private float m_cameraMaxSpeed = 10.0f;
	[SerializeField]
	private float m_cameraMinSpeed = 0.5f;

	[SerializeField]
	private float m_cameraBaseSpeed = 5.0f;

	[SerializeField]
	private float m_cameraSpeedMultiplier = 2.0f;
}
