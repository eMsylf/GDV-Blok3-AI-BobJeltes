using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private float MovementSpeed = 1f;
    private float RotationSpeed = 1f;
    //public Animator anim;

    [Range(.01f, 10f)]
    [SerializeField] private float movementSpeed2 = 6f;
    [SerializeField] private Transform cam;

    private Rigidbody rigidbody_Player;
    private Quaternion rigidbodyQuaternion;
    private Vector3 m_EulerAngleVelocity;

    private float rotation;

    private ObjectInteraction ObjectInteraction;

    [Range(0f, 1f)]
    public float isHoldingMovement = .2f;
    [Range(0f, 1f)]
    public float isHoldingRotation = .0f;

    [NonSerialized] public float isHoldingMovementChange;
    [NonSerialized] public float isHoldingRotationChange;

    private void Start() {
        rigidbody_Player = GetComponent<Rigidbody>();
        ObjectInteraction = GetComponent<ObjectInteraction>();
    }

    private void FixedUpdate() {
        Movement2();
        //Rotation();
    }

    private void Movement() {
        if (Input.GetAxis("Vertical") > 0) {
            rigidbody_Player.AddForce(transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * MovementSpeed);
        }
    }

    private void Rotation() {
        rotation = Input.GetAxis("HorizontalR") * RotationSpeed;

        //if (Input.GetAxis("Vertical") < 0) {
        //    rotation *= -1;
        //}
        m_EulerAngleVelocity = new Vector3(0, rotation, 0);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        rigidbody_Player.MoveRotation(rigidbody_Player.rotation * deltaRotation);
    }

    private void Movement2() {
        //Check if an object is being held. If so, limit movement speed.
        if (ObjectInteraction.isHoldingSomething) {
            isHoldingMovementChange = isHoldingMovement;
            isHoldingRotationChange = isHoldingRotation;
        } else {
            isHoldingMovementChange = 1f;
            isHoldingRotationChange = 1f;
        }

        Vector3 forwardDirection = Vector3.Scale(cam.forward, new Vector3(1, 0, 1));
        Vector3 rightDirection = cam.right;

        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");

        Vector3 velocity = (forwardDirection * vert + rightDirection * hor) * movementSpeed2 * isHoldingMovementChange;
        velocity = Vector3.ClampMagnitude(velocity, movementSpeed2);
        //if (velocity != Vector3.zero)
        //{
        //    anim.SetTrigger("Walk");
        //}
        //else
        //{
        //    anim.ResetTrigger("Walk");
        //}
        rigidbody_Player.MovePosition(rigidbody_Player.position + velocity * Time.deltaTime);
    }


}
