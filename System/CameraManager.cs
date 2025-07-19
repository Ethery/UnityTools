using UnityEngine;
using UnityEngine.InputSystem;
using UnityTools.Systems.Inputs;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private InputManager.Input m_cameraMoveInput;

    [SerializeField] private InputManager.Input m_cameraSpeedUpInput;

    [SerializeField] private InputManager.Input m_cameraSpeedControlInput;

    [SerializeField] private InputManager.Input m_cameraDragInput;

    [SerializeField] private float m_cameraMaxSpeed = 10.0f;

    [SerializeField] private float m_cameraMinSpeed = 0.5f;

    [SerializeField] private float m_cameraBaseSpeed = 5.0f;

    [SerializeField] private float m_cameraSpeedMultiplier = 2.0f;

    [SerializeField] private bool m_isSideView = false;

    private Vector3 m_cameraTargetPos;
    private Plane m_camPlane;
    private Camera m_currentCamera;
    private bool m_dragCamera;
    private Vector3 m_dragGroundReferencePoint;
    private Vector3 m_dragUpdatedGroundPos;
    private Vector3 m_dragUpdatedScreenPos;
    private Plane m_mouseDragPlane;
    private Ray m_newCamPosProjectionRay;
    private Ray m_dragRay;
    private float m_raycastDistance;
    private bool m_speedUp;

    private bool m_updateFrame;

    private Vector2 m_wantedMovement = Vector2.zero;

	InputManager.InputEvent m_OnCameraMovementEvent = null;
    InputManager.InputEvent m_OnCameraMovementEndEvent = null;

    InputManager.InputEvent m_OnCameraSpeedUp_PerformedEvent = null;
    InputManager.InputEvent m_OnCameraSpeedUp_CanceledEvent = null;

    InputManager.InputEvent m_OnCameraSpeedControl_PerformedEvent = null;

    InputManager.InputEvent m_OnCameraDrag_PerformedEvent = null;
    InputManager.InputEvent m_OnCameraDrag_CanceledEvent = null;

	private void OnEnable()
	{
		m_wantedMovement = Vector3.zero;
		m_mouseDragPlane = new Plane(Vector3.up, 0);

		m_OnCameraMovementEvent = new InputManager.InputEvent(OnCameraMovement, InputActionPhase.Performed);
		m_OnCameraMovementEndEvent = new InputManager.InputEvent(OnCameraMovementEnd, InputActionPhase.Canceled);

		m_OnCameraSpeedUp_PerformedEvent = new InputManager.InputEvent(OnCameraSpeedUp, InputActionPhase.Performed);
		m_OnCameraSpeedUp_CanceledEvent = new InputManager.InputEvent(OnCameraSpeedUp, InputActionPhase.Canceled);

		m_OnCameraSpeedControl_PerformedEvent = new InputManager.InputEvent(OnCameraSpeedControl_Performed, InputActionPhase.Performed);

		m_OnCameraDrag_PerformedEvent = new InputManager.InputEvent(OnCameraDrag_Performed, InputActionPhase.Performed);
		m_OnCameraDrag_CanceledEvent = new InputManager.InputEvent(OnCameraDrag_Canceled, InputActionPhase.Canceled);
		RegisterInputs(true);
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

                m_cameraTargetPos.x += m_wantedMovement.x * m_cameraBaseSpeed * Time.deltaTime *
                                       (m_speedUp ? m_cameraSpeedMultiplier : 1);

                if (m_isSideView)
                {
                    m_cameraTargetPos.y += m_wantedMovement.y * m_cameraBaseSpeed * Time.deltaTime *
                                           (m_speedUp ? m_cameraSpeedMultiplier : 1);
                }
                else
                {
                    m_cameraTargetPos.z += m_wantedMovement.y * m_cameraBaseSpeed * Time.deltaTime *
                                           (m_speedUp ? m_cameraSpeedMultiplier : 1);
                }

				m_currentCamera.transform.position = m_cameraTargetPos;
                m_updateFrame = true;
            }

            if (m_dragCamera)
            {

                if (m_mouseDragPlane.Raycast(m_dragRay, out float enter))
                {
                    //Get the point that is clicked
                    m_dragUpdatedGroundPos = m_dragRay.GetPoint(enter);

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
            Debug.Log("No camera");
        }
    }

    private void OnDisable()
    {
        RegisterInputs(false);
    }

    private void RegisterInputs(bool register)
    {
        InputManager.RegisterInput(m_cameraMoveInput
            , m_OnCameraMovementEvent
            , register);
        InputManager.RegisterInput(m_cameraMoveInput
            , m_OnCameraMovementEndEvent
            , register);

        InputManager.RegisterInput(m_cameraSpeedUpInput
            , m_OnCameraSpeedUp_PerformedEvent
            , register);
        InputManager.RegisterInput(m_cameraSpeedUpInput
            , m_OnCameraSpeedUp_CanceledEvent
            , register);

        InputManager.RegisterInput(m_cameraSpeedControlInput
            , m_OnCameraSpeedControl_PerformedEvent
            , register);

        InputManager.RegisterInput(m_cameraDragInput
            , m_OnCameraDrag_PerformedEvent
            , register);
        InputManager.RegisterInput(m_cameraDragInput
            , m_OnCameraDrag_CanceledEvent
            , register);
    }

    private void OnCameraDrag_Performed(InputAction action)
    {
        //Create a ray from the Mouse click position
        m_dragRay = m_currentCamera.ScreenPointToRay(action.ReadValue<Vector2>());
        if (!m_dragCamera)
        {

            if (m_mouseDragPlane.Raycast(m_dragRay, out float enter))
            {
                //Get the point that is clicked
                Vector3 hitPoint = m_dragRay.GetPoint(enter);
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
}