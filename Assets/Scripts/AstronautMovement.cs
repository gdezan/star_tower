using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautMovement : MonoBehaviour {
    public Vector2 movementDirection;
    public float MOVEMENT_BASE_SPEED = 10.0f;
    public Rigidbody2D rb;

    // Update is called once per frame
    void Update() {
        ProcessInputs();
        Move();
    }

    void ProcessInputs() {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void Move() {
        rb.velocity = movementDirection * MOVEMENT_BASE_SPEED;
    }
}
