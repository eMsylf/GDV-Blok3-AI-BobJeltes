using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    private float speed = .2f;
    private Vector3 movement;

    private float movementX, movementZ;
    
    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        movementX = Mathf.Sin(Time.time);
        movementZ = Mathf.Cos(Time.time);

        movement = new Vector3(movementX, 0, movementZ);

        rigidbody.position += movement * speed;
    }
}
