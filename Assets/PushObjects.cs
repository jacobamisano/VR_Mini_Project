using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PushObject : MonoBehaviour
{
    public XRDirectInteractor leftInteractor; // reference to the left hand interactor
    public XRDirectInteractor rightInteractor; // reference to the right hand interactor
    public float pushStrength = 10f; // the strength of the push force

    private Rigidbody objectRigidbody; // reference to the object's rigidbody

    void Start()
    {
        // get the Rigidbody component attached to the object
        objectRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // check if the player is pushing the object using either hand
        ApplyPushForce(leftInteractor);
        ApplyPushForce(rightInteractor);
    }

    void ApplyPushForce(XRDirectInteractor interactor)
    {
        // check if the interactor has selected an object (is interacting with it)
        if (interactor != null && interactor.hasSelection)
        {
            // get the current position of the interactor (the hand's position)
            Vector3 interactorPosition = interactor.transform.position;

            // calculate the distance between the interactor and the object
            float distanceToObject = Vector3.Distance(interactorPosition, transform.position);

            // if the interactor is close enough, apply force to the object
            if (distanceToObject < 1.5f) // you can adjust the range based on your needs
            {
                // calculate the direction from the object to the interactor
                Vector3 pushDirection = interactorPosition - transform.position;

                // apply force to the object in the direction of the interactor
                objectRigidbody.AddForce(pushDirection.normalized * pushStrength);
            }
        }
    }
}
