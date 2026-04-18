using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PressController : MonoBehaviour
{
    [Header("References")]
    public XRGrabInteractable grabInteractable;

    [Header("Input")]
    public Key pressKey = Key.P;

    [Header("Movement Settings")]
    public bool useRotation = false; // toggle between position or rotation
    public float moveSpeed = 5f;

    [Header("Position Mode")]
    public Vector3 pressedLocalPosition;

    [Header("Rotation Mode")]
    public Vector3 pressedLocalRotation;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private bool isGrabbed = false;
    private bool isPressed = false;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
        else
        {
            Debug.LogWarning("XRGrabInteractable not assigned!");
        }
    }

    void Update()
    {
        // Only allow release lever when grabbed
        if (isGrabbed && Keyboard.current != null && Keyboard.current[pressKey].isPressed)
        {
            if (!isPressed)
            {
                Debug.Log("Release Lever Pressed");
            }
            isPressed = true;
        }
        else
        {
            if (isPressed)
            {
                Debug.Log("Release Lever Released");
            }
            isPressed = false;
        }

        // Smooth transition
        if (useRotation)
        {
            Quaternion targetRotation = isPressed
                ? Quaternion.Euler(pressedLocalRotation)
                : initialLocalRotation;

            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * moveSpeed
            );
        }
        else
        {
            Vector3 targetPosition = isPressed
                ? pressedLocalPosition
                : initialLocalPosition;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                targetPosition,
                Time.deltaTime * moveSpeed
            );
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        Debug.Log("Fire Extinguisher Grabbed");
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        Debug.Log("Fire Extinguisher Released");
    }
}