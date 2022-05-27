using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple capsule-collider controller that uses the legacy input system.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region General Fields
    // Externals
    [SerializeField] private PlayerConfigurationSO playerConfigurationSO;
    [Header("Dependencies")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform directionAnchor;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float sprintDuration = 3f;
    [SerializeField] private float sprintCooldownMultiplier = 2f;
    [SerializeField] private float sprintCooldownDelay = 2f;

    // Internals
    private Quaternion _rotation { get; set; } // TODO: This is what controls player's facing-direction / what the camera should parent to.
    private Vector2 mouseInputDelta;
    private Vector3 movementInput;
    private bool isSprintKeyDown;
    private bool isSprinting;
    private float sprintDurationTimer, sprintCooldownDelayTimer;
    private Rigidbody rb;
    #endregion
    
    #region VisualFields
    [SerializeField] private Image sprintCooldownFillImage;
    #endregion

    #region UnityMethods

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
        sprintDurationTimer = sprintDuration;
        sprintCooldownDelayTimer = 0;
    }

    private void Update()
    {
        PollInput();
        CalculateSpeed();
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

        isSprintKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        
        mouseInputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void CalculateSpeed()
    {
        sprintCooldownFillImage.fillAmount = (sprintDurationTimer / sprintDuration);
        // Timers
        if (isSprintKeyDown)
        {
            sprintCooldownDelayTimer = sprintCooldownDelay;
            sprintDurationTimer -= Time.deltaTime;
        }
        if (sprintCooldownDelayTimer > 0)
        {
            sprintCooldownDelayTimer -= Time.deltaTime;
        }
        else
        {
            sprintDurationTimer += Time.deltaTime * sprintCooldownMultiplier;
        }
        sprintDurationTimer = Mathf.Clamp(sprintDurationTimer, 0, sprintDuration);
        
        isSprinting = (sprintDurationTimer > 0) && isSprintKeyDown;
    }
    
    private void CalculateRotation()
    {
        Vector3 rotation = _rotation.eulerAngles;
        Quaternion newRotate = new Quaternion();
        float sens = PlayerConfigurationSO.CameraSensitivity;
        //Debug.Log(sens);
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
        var cameraPivotTransform = directionAnchor.transform;
        var position = playerTransform.position;
        var speed = isSprinting ? movementSpeed * sprintMultiplier : movementSpeed;
        position += cameraPivotTransform.forward * movementInput.z * speed * Time.deltaTime;
        position += cameraPivotTransform.right * movementInput.x * speed * Time.deltaTime;
        rb.MovePosition(position);
    }
    
    private float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }
}
