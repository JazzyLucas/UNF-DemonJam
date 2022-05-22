using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple capsule-collider controller that uses the legacy input system.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Externals
    [SerializeField] private PlayerConfigurationSO playerConfigurationSO;
    [Header("Dependencies")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform directionAnchor;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float movementSpeed = 1;

    // Internals
    private Quaternion _rotation { get; set; } // TODO: This is what controls player's facing-direction / what the camera should parent to.
    private Vector2 mouseInputDelta;
    private Vector3 movementInput;

    #region UnityMethods

    private void Awake()
    {
        
    }

    private void Update()
    {
        PollInput();
        CalculateRotation();
        
        ApplyRotation();
        ApplyMovement();
    }

    #endregion
    
    private void PollInput()
    {
        movementInput = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W))
            movementInput += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            movementInput += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            movementInput += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            movementInput += Vector3.right;
        
        mouseInputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
    
    private void CalculateRotation()
    {
        Vector3 rotation = _rotation.eulerAngles;
        Quaternion newRotate = new Quaternion();
        float sens = PlayerConfigurationSO.CameraSensitivity;
        Debug.Log(sens);
        rotation += new Vector3(
            playerConfigurationSO.InvertX ? 1 : -1 * mouseInputDelta.y * sens * Time.deltaTime, 
            playerConfigurationSO.InvertY ? -1 : 1 * mouseInputDelta.x * sens * Time.deltaTime, 
            0f);
        float clampedX = ClampAngle(rotation.x, -20f, 20f);
        
        newRotate.eulerAngles = new Vector3(clampedX, rotation.y, 0f);
        _rotation = newRotate;
    }

    private void ApplyRotation()
    {
        cameraPivot.rotation = _rotation;
    }

    private void ApplyMovement()
    {
        Transform cameraPivotTransform = directionAnchor.transform;
        Vector3 position = playerTransform.position;
        position += cameraPivotTransform.forward * movementInput.z * movementSpeed * Time.deltaTime;
        position += cameraPivotTransform.right * movementInput.x * movementSpeed * Time.deltaTime;
        playerTransform.position = position;
    }
    
    private float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
