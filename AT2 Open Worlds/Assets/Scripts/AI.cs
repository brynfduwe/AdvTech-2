using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour
{
    public enum State
    {
        Idle,
        Moving,
        Fleeing,
        Dead
    }

    public float speed;
    public float runSpeed;
    public State CurrentState;
    public int id;
    public int originZone;
    public int zone;

    public Transform[] nodes;
    public Transform currentNode;
    public float range;

    private float idleTimer;
    private float idleAmount;
    private bool idleTimeSet;

    Transform player;

    private Vector3 fleeDir;

	// Use this for initialization
	void Start ()
	{
	    player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    if (Input.GetKeyDown(KeyCode.P))
	    {
            if (Vector3.Distance(transform.position, player.transform.position) < 15)
	        {
	            if (CurrentState != State.Dead)
	            {
	                setDead();
                }
	        }
	    }

	//    Debug.DrawRay(this.transform.position, Vector3.up * 25, Color.cyan);

	    switch (CurrentState)
	    {
	        case State.Idle:

	            if (idleTimeSet)
	            {
	                idleTimer += Time.deltaTime;
	                if (idleTimer > idleAmount)
	                {
	                    idleTimer = 0;
	                    idleTimeSet = false;
	                    CurrentState = State.Moving;
	                }
	            }
	            else
	            {
	                idleAmount = Random.Range(3, 7);
	                idleTimeSet = true;
	            }

	            break;

	        case State.Moving:

	            GetComponent<Rigidbody>().AddForce(transform.forward * speed);

	            if (Physics.Raycast(transform.position, transform.forward, 10))
	            {
	                if ((Random.Range(0, 10)) > 5)
	                {
	                    CurrentState = State.Idle;
	                }
	                else
	                {
	                    transform.Rotate(transform.up, Random.Range(0, 360));
	                }
	            }

	            break;

	        case State.Fleeing:

	            GetComponent<Rigidbody>().AddForce(transform.forward * runSpeed);

	            // transform.position = Vector2.MoveTowards(transform.position, player.position, -1 * Time.deltaTime);

	            if (Physics.Raycast(transform.position, transform.forward, 50))
	            {
	                Flee();
	            }

	            break;

	        case State.Dead:

                //dead- do nothing
                //fall over

	            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
	                new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z), Time.deltaTime * 3);

	            break;
	    }


	    if (currentNode != null)
	    {
	        if (Vector3.Distance(transform.position, currentNode.transform.position) > range)
	        {
	            FindNearestNode();
	        }
	    }
	    else
	    {
	        FindNearestNode();
        }
	}


    public void FindNearestNode()
    {
        float bestdist = 100000;

        if (currentNode != null)
        {
            currentNode.GetComponent<ZoneNode>().AIObjectList.Remove(this.transform);
        }

        for (int i = 1; i < nodes.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, nodes[i].transform.position);

            if (dist < bestdist)
            {
                bestdist = dist;
                zone = nodes[i].GetComponent<ZoneNode>().loadZone;
                currentNode = nodes[i];
            }
        }

       // Debug.Log("moved to: " + currentNode.name + currentNode.GetComponent<ZoneNode>().loadZone.ToString());

        if (currentNode != null)
        {
            currentNode.GetComponent<ZoneNode>().AddToAIList(this.transform);
        }
    }


    void Flee()
    {
        Vector3 oldRot = transform.eulerAngles;

        transform.Rotate(transform.up, Random.Range(0, 360));

      //  Vector3 Dir = player.position - transform.position;
     //   float AIAngle = Vector3.Dot(Dir.normalized, transform.forward);

        //if (AIAngle < 0.35f)
        //{
        //    transform.Rotate(transform.up, Random.Range(0, 360));

        //    Dir = player.position - transform.position;
        //    AIAngle = Vector3.Dot(Dir.normalized, transform.forward);

        //    if (AIAngle < 0.35f)
        //    {
        //        transform.Rotate(transform.up, Random.Range(0, 360));
        //    }
        //}

      //  fleeDir = transform.eulerAngles;
      //  transform.eulerAngles = oldRot;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (CurrentState != State.Dead)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 60);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].GetComponent<AI>() != null)
                    {
                        hitColliders[i].GetComponent<AI>().setFlee();
                    }
                }

                setFlee();
            }
        }
        else
        {
            if(CurrentState == State.Fleeing)
                Flee();
        }
    }


    public void setFlee()
    {
        if (CurrentState != State.Dead)
        {
            CurrentState = State.Fleeing;
            GetComponent<Renderer>().material.color = Color.yellow;
            Flee();
        }
    }


    public void setDead()
    {
        if (CurrentState != State.Dead)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 60);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].GetComponent<AI>() != null)
                {
                    hitColliders[i].GetComponent<AI>().setFlee();
                }
            }
        }

        CurrentState = State.Dead;
            GetComponent<Renderer>().material.color = Color.red;
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        
    }


    public void SetStateOnSpawn(int _state, int _id, int _originZone)
    {
        id = _id;
        originZone = _originZone;
        CurrentState = (State) _state;
        Debug.Log("Spawned state: " + CurrentState.ToString());

        switch (CurrentState)
        {
            case State.Fleeing:
                setFlee();
                break;

            case State.Dead:
                setDead(); 
                break;
        }
    }
}
