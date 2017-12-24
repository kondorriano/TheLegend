using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {


    public int id = 1;

    public Transform playerModel;
    public float initialSpeed = 7f;
    public float initialGravity = -20f;
    float speed;
    float enviromentSpeed = 1;

    Animator anim;
    Rigidbody myRigidbody;
    Vector3 walkDirection = Vector3.zero;


    public void setId(int myId)//this is for setting the ID
    {
        id = myId;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
        speed = initialSpeed;
    }

	void Update()//This is for updating
    {
        bool up = false;
        bool down = false;
        bool right = false;
        bool left = false;
        float xAxis = Input.GetAxis("Horizontal" + id);
        float yAxis = Input.GetAxis("Vertical" + id);
        if (Mathf.Abs(xAxis) < 0.15f) xAxis = 0;
        if (Mathf.Abs(yAxis) < 0.15f) yAxis = 0;

        walkDirection = new Vector3(xAxis, 0, yAxis);

        up = (yAxis >= 0.15f);
        down = (yAxis <= -0.15f);
        left = (xAxis <= -0.15f);
        right = (xAxis >= 0.15f);

        bool isMoving = (up || down || left || right);

        //Movement
        Vector3 velocity = new Vector3(xAxis, 0, yAxis) * speed * enviromentSpeed;
        Vector3 gravity = new Vector3(0, myRigidbody.velocity.y + initialGravity * Time.deltaTime, 0);
        myRigidbody.velocity = velocity + gravity;
        playerModel.LookAt(transform.position + walkDirection.normalized);
    }
}
