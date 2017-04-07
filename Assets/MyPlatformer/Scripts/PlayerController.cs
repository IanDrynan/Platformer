using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float wallSpeed;
    public float moveSpeed;
    public float jumpHeight;
    public float dashSpeed;
    public float dashTime;
    public int actionSet;
    public int actionCount;
    private bool isDashing;
    private bool isJumping;
    private Rigidbody rb;
    private float distGround;
    private float distWall;
    private float speed;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        distGround = GetComponent<Collider>().bounds.extents.y;
        distWall = GetComponent<Collider>().bounds.extents.x;
        actionCount = actionSet;
        speed = moveSpeed;
	}

	void Update()
    {
        if (onWall())
        {
            actionCount = actionSet;
        }
        if (isGrounded())
        {
            actionCount = actionSet;
        }
        if (Input.GetMouseButtonDown(0)) //if screen is touched. 
        {
            if (actionCount > 0) //Double jump/dash counter
            {
                if (Input.mousePosition.x > (Screen.width * .5)) //jump
                {
                    isDashing = false;
                    isJumping = true;
                    rb.velocity = new Vector2(moveSpeed, 0);
                    rb.AddForce(new Vector2(0, jumpHeight), ForceMode.Impulse);
                    actionCount -= 1;
                }
                else if (Input.mousePosition.x < (Screen.width * .5) && !isDashing) //dash
                {
                    isDashing = true;
                    isJumping = false;
                    StartCoroutine(dash(dashSpeed, dashTime));
                    actionCount -= 1;
                }
            }

        }
    }
	// Update is called once per frame - Physics such as gravity should be here
	void FixedUpdate () {
        if (isGrounded() && !isDashing && !onWall())  //constantly move at constant speed 
        {
            rb.velocity = new Vector2(moveSpeed, 0);
        }
        if (!isGrounded() && !isDashing) //apply gravity while grounded and not dashing
        {
            if (onWall()) //fall speed should be constant while sliding on a wall
            {
                if (-rb.velocity.y > wallSpeed)
                {
                    rb.velocity = rb.velocity.normalized * wallSpeed;
                }
                else
                {
                rb.AddForce(Physics.gravity * 1);
                }
                
            }
            else {
                rb.AddForce(Physics.gravity * 2);
            }
        }
	}

    bool isGrounded() 
    {
        //Debug.DrawRay(transform.position, -Vector3.up, Color.green, 1f, false);
        if (Physics.Raycast((transform.position - new Vector3(distWall - .1f, 0, 0)), -Vector3.up, distGround + .01f) ||  //ray from left side
            Physics.Raycast((transform.position), -Vector3.up, distGround + .01f) ||                               //ray from middle
            Physics.Raycast((transform.position + new Vector3(distWall - .11f, 0, 0)), -Vector3.up, distGround + .01f))  //ray from right side
        {
            //Debug.Log("ground");
            return true;
        }
        else return false;
    }
    
    bool onWall()
    {
        
        if (Physics.Raycast(transform.position - new Vector3(0, distGround - .1f, 0), Vector3.right, distWall + .01f) || //ray from bottom
            Physics.Raycast(transform.position, Vector3.right, distWall + .01f) ||                                //ray from middle
            Physics.Raycast(transform.position + new Vector3(0, distGround - .1f, 0), Vector3.right, distWall + .01f)) //ray from top
        {
            moveSpeed = -speed;
            dashSpeed = -speed;
            //Debug.Log("wallright");
            return true;
        }
        else if (Physics.Raycast(transform.position - new Vector3(0, distGround - .1f, 0), -Vector3.right, distWall + .01f) || //ray from bottom
            Physics.Raycast(transform.position, -Vector3.right, distWall + .01f) ||                                //ray from middle
            Physics.Raycast(transform.position + new Vector3(0, distGround - .1f, 0), -Vector3.right, distWall + .01f)) //ray from top
        {
            moveSpeed = speed;
            dashSpeed = speed;
            //Debug.Log("wallleft"); 
            return true;
        }
        else {
            return false;
        }
    }
    
    IEnumerator dash(float d_speed, float d_time)
    {
        float time = 0f;

        while(d_time > time)
        {
            time += Time.deltaTime;
            rb.velocity = new Vector2(d_speed, 0);
            yield return 0;
        }
        isDashing = false;
    }
}
