using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{

    public float Speed = 5.0f;

    Rigidbody rigidbody;

    Vector3 movement;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();


    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        movement.Set(h, 0, v);
        movement = movement.normalized * Speed * Time.deltaTime;

        rigidbody.MovePosition(transform.position + movement);

    }
    
}
