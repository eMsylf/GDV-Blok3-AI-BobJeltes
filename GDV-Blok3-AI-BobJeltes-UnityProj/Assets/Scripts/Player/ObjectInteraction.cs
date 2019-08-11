using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour {

    [SerializeField] private Transform artTransform;
    [Range(0f, 10f)]
    [SerializeField] private float maxInteractionRange = 2f;
    [Range(1f, 3f)]
    [SerializeField] private float pickupHoldDistance = 2f;
    [SerializeField] private float rotationAmount = 30f;

    // Face buttons
    KeyCode a = KeyCode.Joystick1Button0;
    KeyCode b = KeyCode.Joystick1Button1;
    KeyCode x = KeyCode.Joystick1Button2;
    KeyCode y = KeyCode.Joystick1Button3;

    // Bumper buttons
    KeyCode lb = KeyCode.Joystick1Button4;
    KeyCode rb = KeyCode.Joystick1Button5;

    private Ray ray;
    private RaycastHit hitInfo;

    private Transform pickup;
    private Rigidbody pickup_rb;

    public bool isHoldingSomething;

    void Start() {

    }

    void Update() {
        Physics.Raycast(new Ray(artTransform.position, artTransform.forward), out hitInfo, maxInteractionRange);

        // When the player is already holding an object
        if (pickup != null) {
            if (Input.GetKeyDown(a)) {
                DropObject();
            }
            if (isHoldingSomething) {
                if (Input.GetKeyDown(lb)) {
                    TurnObjectLeft();
                }
                if (Input.GetKeyDown(rb)) {
                    TurnObjectRight();
                }
            }
        } // When the player is not holding an object
        else if (pickup == null) {
            if (hitInfo.transform != null) {
                if (hitInfo.transform.GetComponent<PickupObject>() != null) {
                    if (Input.GetKeyDown(a)) {
                        PickupObject();
                    }
                }
                // If there's a switch to press, toggle it
                //if (hitInfo.transform.GetComponent<FanSwitch>() != null) {
                //    if (Input.GetKeyDown(a)) {
                //        FanSwitch fanSwitch = hitInfo.transform.GetComponent<FanSwitch>();
                //        fanSwitch.Toggle();
                //    }
                //}
            }
        }
    }

    private void DropObject() {
        isHoldingSomething = false;

        Debug.Log("Drop " + pickup.name);
        pickup_rb.isKinematic = false;
        pickup.SetParent(null);
        pickup = null;
    }

    private void TurnObjectLeft() {
        pickup.transform.Rotate(0, -rotationAmount, 0);
        Debug.Log("L1, turn object LEFT");
    }

    private void TurnObjectRight() {
        pickup.transform.Rotate(0, rotationAmount, 0);
        Debug.Log("R1, turn object RIGHT");
    }

    private void PickupObject() {
        pickup = hitInfo.transform;
        // Check if the pickup object is a blowing fan. If so, the player can't pick it up.
        //if (pickup.GetComponent<Fan>() != null) {
        //    if (pickup.GetComponentInChildren<Wind_Script>().isBlowing) {
        //        return;
        //    }
        //}

        isHoldingSomething = true;

        Debug.Log("Pickup " + hitInfo.transform.name);
        // Store variables
        pickup_rb = pickup.GetComponent<Rigidbody>();

        // Move position to 
        pickup.transform.position = artTransform.position + artTransform.forward * pickupHoldDistance;
        pickup_rb.isKinematic = true;

        pickup.SetParent(artTransform);
    }

}
