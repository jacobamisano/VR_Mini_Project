using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ClimbScript : MonoBehaviour
{
    public XRGrabInteractable grabInteractable; // reference to the grab interactable object (the object the player can grab)
    public Transform player; // reference to the player transform (the VR camera or XR Rig)
    public Rigidbody playerRigidbody; // reference to player's Rigidbody to disable physics during climb

    private bool isClimbing = false; // flag to check if the player is currently climbing
    private Vector3 initialPlayerPosition; // stores the initial position of the player when climbing starts

    public float climbSpeed = 2f; // speed at which the player climbs (you can adjust this based on preference)

    private InputDevice leftDevice; // reference to the left XR input device
    private InputDevice rightDevice; // reference to the right XR input device

    void Start()
    {
        // subscribe to selectEntered and selectExited events using the updated method
        grabInteractable.selectEntered.AddListener(StartClimbing);
        grabInteractable.selectExited.AddListener(StopClimbing);

        // Get the input devices for left and right controllers
        InitializeXRDevices();
    }

    // called when the player grabs the object
    void StartClimbing(SelectEnterEventArgs args)
    {
        isClimbing = true; // set isClimbing flag to true when the player grabs the object
        initialPlayerPosition = player.position; // store the initial player position when grabbing the object

        // set the player's Rigidbody to kinematic to disable physics during climbing
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = true;
        }

        // you can disable regular movement here if needed (e.g., disable player movement script)
    }

    // called when the player releases the object
    void StopClimbing(SelectExitEventArgs args)
    {
        isClimbing = false; // set isClimbing flag to false when the player releases the object

        // re-enable physics interaction after releasing the object
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;
        }

        // re-enable regular movement here if needed (e.g., enable player movement script)
    }

    void Update()
    {
        if (isClimbing)
        {
            // get climbing input from the controllers (thumbstick or grip input)
            // left controller input
            float leftClimbInput = GetClimbingInput(leftDevice);

            // right controller input
            float rightClimbInput = GetClimbingInput(rightDevice);

            // use the average of both controller inputs to decide vertical climb movement
            float climbInput = (leftClimbInput + rightClimbInput) / 2;

            // move the player smoothly based on climb input
            player.position = Vector3.Lerp(initialPlayerPosition, initialPlayerPosition + new Vector3(0, climbInput * climbSpeed, 0), Time.deltaTime);
        }
    }

    // helper method to get climbing input from the controller
    float GetClimbingInput(InputDevice device)
    {
        // check if the device has thumbstick or button input available
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstickInput))
        {
            // return the vertical value (Y-axis) of the thumbstick for climbing input
            return thumbstickInput.y;
        }

        // if no input is available, return 0
        return 0f;
    }

    // helper method to initialize XR input devices
    void InitializeXRDevices()
    {
        // find left and right input devices (adjust based on your VR hardware)
        var leftHandedDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        var rightHandedDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (leftHandedDevice.isValid)
        {
            leftDevice = leftHandedDevice;
        }

        if (rightHandedDevice.isValid)
        {
            rightDevice = rightHandedDevice;
        }
    }
}
