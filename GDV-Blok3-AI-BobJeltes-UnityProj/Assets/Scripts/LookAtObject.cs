using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour {
    [Tooltip("This defaults to the main camera, if left empty")] public Transform LookTarget;

    void Start() {
        if (LookTarget == null) {
            Debug.Log("Assigning camera as look target");
            LookTarget = Camera.main.transform;
        }
    }

    void Update() {
        transform.LookAt(LookTarget);
    }
}
