using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityTools.Systems.Inputs;

public class CameraManager : Singleton<CameraManager>
{
	private void OnEnable()
	{
		RegisterInputs(true);
		m_wantedMovement = Vector3.zero;
		m_mouseDragPlane = new Plane(Vector3.up, 0);
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
									, new InputManager.InputEvent(OnCameraSpeedControl_Performed, InputActionPhase.Performed)
									, register);

		InputManager.RegisterInput(m_cameraDragInput
                                    , new InputManager.InputEvent(OnCameraDrag_Performed, InputActionPhase.Performed)
									, register);
		InputManager.RegisterInput(m_cameraDragInput
                                    , new InputManager.InputEvent(OnCameraDrag_Canceled, InputActionPhase.Canceled)
									, register);
	}

    private void OnCameraDrag_Performed(InputAction action)
    {
		if(!m_dragCamera)
		{
			//Create a ray from the Mouse click position
			Ray ray = m_currentCamera.ScreenPointToRay(Input.mousePosition);

			if (m_mouseDragPlane.Raycast(ray, out float enter))
			{
				//Get the point that is clicked
				Vector3 hitPoint = ray.GetPoint(enter);
				m_dragCamera = true;
				m_dragGroundReferencePoint = hitPoint;
			}
		}
    }
	
    private void OnCameraDrag_Canceled(InputAction action)
    {
		m_dragCamera = false;
    }

    public void OnCameraMovement(InputAction action)
	{
		m_wantedMovement = action.ReadValue<Vector2>();
	}
	public void OnCameraMovementEnd(InputAction action)
	{
		m_wantedMovement = Vector2.zero;
	}

	public void OnCameraSpeedUp(InputAction action)
	{
		m_speedUp = action.ReadValue<float>() > 0f;
	}

	public void OnCameraSpeedControl_Performed(InputAction action)
	{
		float actionValue = action.ReadValue<float>();
		actionValue = Mathf.Clamp(actionValue, -0.2f, 0.2f);
		m_cameraBaseSpeed += actionValue;
		m_cameraBaseSpeed = Mathf.Clamp(m_cameraBaseSpeed, m_cameraMinSpeed, m_cameraMaxSpeed);
	}

	private void LateUpdate()
	{
		if (m_currentCamera == null)
			m_currentCamera = Camera.main;
		if (m_currentCamera != null)
		{
			if (m_wantedMovement != Vector2.zero)
            {
                m_cameraTargetPos = m_currentCamera.transform.position;
                m_cameraTargetPos.x += m_wantedMovement.x * m_cameraBaseSpeed * Time.deltaTime * (m_speedUp ? m_cameraSpeedMultiplier : 1);
                m_cameraTargetPos.z += m_wantedMovement.y * m_cameraBaseSpeed * Time.deltaTime * (m_speedUp ? m_cameraSpeedMultiplier : 1);

                m_currentCamera.transform.position = m_cameraTargetPos;
                m_updateFrame = true;
            }

			if(m_dragCamera)
            { 
				//Create a ray from the Mouse click position
                Ray ray = m_currentCamera.ScreenPointToRay(Input.mousePosition);

                if (m_mouseDragPlane.Raycast(ray, out float enter))
                {
                    //Get the point that is clicked
                    m_dragUpdatedGroundPos = ray.GetPoint(enter);

                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = m_currentCamera.nearClipPlane;
                    m_dragUpdatedScreenPos = m_currentCamera.ScreenToWorldPoint(mousePosition);

					Vector3 camDirection = m_dragUpdatedScreenPos - m_dragUpdatedGroundPos;

                    m_camPlane = new Plane(Vector3.down, m_currentCamera.transform.position.y);
					m_newCamPosProjectionRay = new Ray(m_dragGroundReferencePoint, camDirection);
                    m_camPlane.Raycast(m_newCamPosProjectionRay, out m_raycastDistance);

					
					m_cameraTargetPos = m_newCamPosProjectionRay.GetPoint(m_raycastDistance);
					m_updateFrame = true;
                }
            }
			if (m_updateFrame)
			{
				m_currentCamera.transform.position = m_cameraTargetPos;
				m_updateFrame = false;
			}
		}
		else
		{
			Debug.Log($"No camera");
		}
	}

    private void OnDrawGizmos()
    {
		/*Gizmos.color = Color.blue;
		Gizmos.DrawSphere(m_dragGroundReferencePoint, 0.5f);
        Gizmos.DrawSphere(m_dragBeginScreenPos, 0.1f);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(m_dragUpdatedGroundPos, 0.25f);
        Gizmos.DrawSphere(m_dragUpdatedScreenPos, 0.1f);
        Gizmos.DrawLine(m_dragGroundReferencePoint, m_dragUpdatedGroundPos);
        Gizmos.DrawLine(m_dragGroundReferencePoint, m_dragBeginScreenPos);
        Gizmos.DrawLine(m_dragUpdatedGroundPos, m_dragUpdatedScreenPos);

        Gizmos.color = Color.green;
		Gizmos.DrawCube(-m_camPlane.normal * m_camPlane.distance, Vector3.one * 0.2f);
		Gizmos.DrawLine(-m_camPlane.normal * m_camPlane.distance, m_camPlane.normal);
		Gizmos.DrawSphere(m_cameraTargetPos, 0.1f);

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(m_newCamPosProjectionRay.origin, m_newCamPosProjectionRay.origin+(m_newCamPosProjectionRay.direction* m_raycastDistance));
		Gizmos.DrawLine(m_newCamPosProjectionRay.origin, m_newCamPosProjectionRay.origin+(m_newCamPosProjectionRay.direction));

		Gizmos.color = Color.white;*/
	}

    private void OnDisable()
	{
		RegisterInputs(false);
	}

	private Vector2 m_wantedMovement = Vector2.zero;
    private Camera m_currentCamera = null;
    private Plane m_mouseDragPlane;
    private bool m_speedUp = false;

	private bool m_updateFrame;
	private bool m_dragCamera;
	private Vector3 m_dragGroundReferencePoint;
	private Vector3 m_cameraTargetPos;
	private Vector3 m_dragUpdatedScreenPos;
	private Vector3 m_dragUpdatedGroundPos;
	private Ray m_newCamPosProjectionRay;
	private Plane m_camPlane;
	private float m_raycastDistance;

	[SerializeField] 
	private InputManager.Input m_cameraMoveInput;
	[SerializeField] 
	private InputManager.Input m_cameraSpeedUpInput;
	[SerializeField] 
	private InputManager.Input m_cameraSpeedControlInput;
	[SerializeField]
	private InputManager.Input m_cameraDragInput;

	[SerializeField] 
	private float m_cameraMaxSpeed = 10.0f;
	[SerializeField] 
	private float m_cameraMinSpeed = 0.5f;

	[SerializeField] 
	private float m_cameraBaseSpeed = 5.0f;

	[SerializeField] 
	private float m_cameraSpeedMultiplier = 2.0f;

}
