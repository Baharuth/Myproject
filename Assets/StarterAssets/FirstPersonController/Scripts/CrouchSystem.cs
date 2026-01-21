using UnityEngine;
using StarterAssets;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(FirstPersonController))]
[RequireComponent(typeof(CharacterController))]
public class CrouchSystem : MonoBehaviour
{
    [Header("Crouch Settings")]
    public float CrouchHeight = 1.2f;
    public float CrouchSpeed = 2.0f;
    public float TransitionSpeed = 10.0f;

    [Header("Camera Adjustment")]
    public float CameraHeightOffset = 0.6f;

    [Header("Collision Checks")]
    public LayerMask CeilingLayerMask;
    public float CeilingCheckOffset = 0.1f;

    // Reference
    private FirstPersonController _fpc;
    private CharacterController _controller;
    // _input zde už nepotøebujeme pro krèení, ale necháme ho pro kompatibilitu

    // Pùvodní hodnoty
    private float _standingHeight;
    private Vector3 _standingCenter;
    private float _defaultMoveSpeed;
    private float _defaultSprintSpeed;
    private Vector3 _initialCameraPosition;

    // Stav
    private bool _isCrouching;
    private float _targetHeight;
    private Vector3 _targetCenter;
    private Vector3 _targetCameraPosition;

    private void Start()
    {
        _fpc = GetComponent<FirstPersonController>();
        _controller = GetComponent<CharacterController>();

        _standingHeight = _controller.height;
        _standingCenter = _controller.center;
        _defaultMoveSpeed = _fpc.MoveSpeed;
        _defaultSprintSpeed = _fpc.SprintSpeed;

        if (_fpc.CinemachineCameraTarget != null)
        {
            _initialCameraPosition = _fpc.CinemachineCameraTarget.transform.localPosition;
        }

        _targetHeight = _standingHeight;
        _targetCenter = _standingCenter;
        _targetCameraPosition = _initialCameraPosition;
    }

    private void Update()
    {
        HandleCrouchInput();
        ApplyCrouchTransition();
    }

    private void HandleCrouchInput()
    {
        // ZDE JE ZMÌNA: Ptáme se pøímo na klávesu Left Ctrl
        bool isCtrlPressed = false;

#if ENABLE_INPUT_SYSTEM
        // Pokud je pøipojená klávesnice, zkontroluj Levý Ctrl
        if (Keyboard.current != null)
        {
            isCtrlPressed = Keyboard.current.leftCtrlKey.isPressed;
        }
#else
        // Fallback pro starý input systém (kdyby náhodou)
        isCtrlPressed = Input.GetKey(KeyCode.LeftControl);
#endif

        if (isCtrlPressed)
        {
            StartCrouch();
        }
        else
        {
            if (CanStandUp())
            {
                StopCrouch();
            }
        }
    }

    private void StartCrouch()
    {
        if (_isCrouching) return;
        _isCrouching = true;

        _targetHeight = CrouchHeight;
        float heightDifference = _standingHeight - CrouchHeight;
        _targetCenter = _standingCenter - new Vector3(0, heightDifference / 2, 0);

        _fpc.MoveSpeed = CrouchSpeed;
        _fpc.SprintSpeed = CrouchSpeed;

        _targetCameraPosition = _initialCameraPosition - new Vector3(0, CameraHeightOffset, 0);
    }

    private void StopCrouch()
    {
        if (!_isCrouching) return;
        _isCrouching = false;

        _targetHeight = _standingHeight;
        _targetCenter = _standingCenter;

        _fpc.MoveSpeed = _defaultMoveSpeed;
        _fpc.SprintSpeed = _defaultSprintSpeed;

        _targetCameraPosition = _initialCameraPosition;
    }

    private void ApplyCrouchTransition()
    {
        _controller.height = Mathf.Lerp(_controller.height, _targetHeight, Time.deltaTime * TransitionSpeed);
        _controller.center = Vector3.Lerp(_controller.center, _targetCenter, Time.deltaTime * TransitionSpeed);

        if (_fpc.CinemachineCameraTarget != null)
        {
            _fpc.CinemachineCameraTarget.transform.localPosition = Vector3.Lerp(
                _fpc.CinemachineCameraTarget.transform.localPosition,
                _targetCameraPosition,
                Time.deltaTime * TransitionSpeed
            );
        }
    }

    private bool CanStandUp()
    {
        float radius = _controller.radius * 0.9f;
        float distance = _standingHeight - _controller.height + CeilingCheckOffset;
        Vector3 startOrigin = transform.position + new Vector3(0, _controller.height, 0);

        if (Physics.SphereCast(startOrigin, radius, Vector3.up, out RaycastHit hit, distance, CeilingLayerMask))
        {
            return false;
        }
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (_controller == null) return;
        Gizmos.color = Color.yellow;
        Vector3 startOrigin = transform.position + new Vector3(0, _controller.height, 0);
        float distance = (_standingHeight - _controller.height) + CeilingCheckOffset;
        Gizmos.DrawLine(startOrigin, startOrigin + Vector3.up * distance);
        Gizmos.DrawWireSphere(startOrigin + Vector3.up * distance, _controller.radius * 0.9f);
    }
}