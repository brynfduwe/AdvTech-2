using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float speedUp;
    public float maxSpeed;
    public float sprintSpeed;
    public float rotateSpeed;
    public float jumpForce;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    float x = Input.GetAxis("Horizontal");
	    float z = Input.GetAxis("Vertical");

	    Vector3 magnitude = transform.forward * z + transform.right * x;

	    magnitude = magnitude.normalized * speedUp;

	    GetComponent<Rigidbody>().AddForce(magnitude);

	    float currentMaxSpeed = maxSpeed;

	    if (Input.GetKey(KeyCode.LeftShift))
	        currentMaxSpeed = sprintSpeed;
       

        if (GetComponent<Rigidbody>().velocity.x > currentMaxSpeed)
	        GetComponent<Rigidbody>().velocity = new Vector3(currentMaxSpeed, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);

	    if (GetComponent<Rigidbody>().velocity.x < -currentMaxSpeed)
	        GetComponent<Rigidbody>().velocity = new Vector3(-currentMaxSpeed, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);


        if (GetComponent<Rigidbody>().velocity.z > currentMaxSpeed)
	        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, currentMaxSpeed);

	    if (GetComponent<Rigidbody>().velocity.z < -currentMaxSpeed)
	        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, -currentMaxSpeed);


        //rotation
        if (Input.GetKey(KeyCode.E))
	    {
	        transform.Rotate(Vector3.up * (Time.deltaTime * rotateSpeed));
        }

	    if (Input.GetKey(KeyCode.Q))
	    {
	        transform.Rotate(-Vector3.up * (Time.deltaTime * rotateSpeed));
        }

        //jump
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
