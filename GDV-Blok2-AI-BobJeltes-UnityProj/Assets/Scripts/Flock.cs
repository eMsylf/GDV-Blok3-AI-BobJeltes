using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public float speed = .1f;

    void Start()
    {
        speed = Random.Range(.5f, 1f);
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * speed, 0, 0);
    }
}
