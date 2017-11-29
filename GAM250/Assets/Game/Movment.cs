using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movment : MonoBehaviour
{
    public float speed = 4;

    Rigidbody rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>(); //Grab Rigidbody from player object
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal") * speed; //Get the horizontal axis input and multiply it by the speed
        float v = Input.GetAxis("Vertical") * speed;    //Get the vertical axis input and multiply it by the speed

        rBody.velocity = new Vector3(h,rBody.velocity.y,v); //Set the y velocity to it's self so we don't change gravity
    }
}
