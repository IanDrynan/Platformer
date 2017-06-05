using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    public float wallSpeed;
    public float moveSpeed;
    public float jumpHeight;
    public float dashSpeed;
    public float dashTime;
    public float gravityWall;
    public float gravityMultiplier;
    public int actionSet;
    private int actionCount;
    private bool isDashing;
    private Rigidbody rb;
    private float distGround;
    private float distWall;
    private float mspeed;
    private float dspeed;
    private float dashTimer;
    //private Animator cubeAnim;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        //cubeAnim = transform.FindChild("Cube").GetComponent<Animator>();
        distGround = GetComponent<Collider>().bounds.extents.y;
        distWall = GetComponent<Collider>().bounds.extents.x;
        actionCount = actionSet;
        mspeed = moveSpeed;
        dspeed = dashSpeed;
    }

	void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
        {

        }
        else {
            if (Input.GetMouseButtonDown(0)) //if screen is touched. 
            {
                if (actionCount > 0) //Double jump/dash counter
                {

                    if (Input.mousePosition.x > (Screen.width * .5)) //jump
                    {
                        actionCount -= 1;
                        isDashing = false;
                        dashTimer = dashTime + 1f;
                        rb.velocity = new Vector2(moveSpeed, 0);
                        rb.AddForce(new Vector2(0, jumpHeight), ForceMode.Impulse);
                        //cubeAnim.Play("Jump");
                    }
                    else if (Input.mousePosition.x < (Screen.width * .5) && !isDashing) //dash
                    {
                        actionCount -= 1;
                        isDashing = true;
                        dashTimer = 0f;
                        StartCoroutine(dash(dashSpeed, dashTime));
                    }
                }

            }
        }
    }

    void LateUpdate () //late so that actioncount doesnt get reset after jumping 
    {
        if (onWall())
        {
            //Debug.Log("adding jump wall");
            actionCount = actionSet;
        }
        if (isGrounded())
        {
            //Debug.Log("adding jump ground");
            actionCount = actionSet;
        }
    }
	// Update is called once per frame - Physics such as gravity should be here
	void FixedUpdate () {
        //Debug.Log(dashTimer);
        if (isGrounded() && !isDashing && !onWall()) //constantly move at constant speed 
        {
            rb.velocity = new Vector2(moveSpeed, 0);
        }
        else if (rb.velocity.magnitude < .3f && !isGrounded() && !onWall()) //edge case where none of the raycasts can detect a ground or a wall and is stuck at a corner 
        {
            //Debug.Log("edge case");
            rb.transform.Translate(0,.1f,0);
            rb.velocity = new Vector2(moveSpeed, 0);
        }
        else if (!isGrounded() && !isDashing) //apply gravity while not grounded and not dashing
        {
            if (onWall()) //fall speed should be constant while sliding on a wall
            {
                if (-rb.velocity.y > wallSpeed)
                {
                    rb.velocity = rb.velocity.normalized * wallSpeed;
                }
                else
                {
                rb.AddForce(Physics.gravity * gravityWall);
                }
            }
            else {
                rb.AddForce(Physics.gravity * gravityMultiplier);
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
            moveSpeed = -mspeed;
            dashSpeed = -dspeed;
            //Debug.Log("wallright");
            return true;
        }
        else if (Physics.Raycast(transform.position - new Vector3(0, distGround - .1f, 0), -Vector3.right, distWall + .01f) || //ray from bottom
            Physics.Raycast(transform.position, -Vector3.right, distWall + .01f) ||                                //ray from middle
            Physics.Raycast(transform.position + new Vector3(0, distGround - .1f, 0), -Vector3.right, distWall + .01f)) //ray from top
        {
            moveSpeed = mspeed;
            dashSpeed = dspeed;
            //Debug.Log("wallleft"); 
            return true;
        }
        else {
            return false;
        }
    }
    
    IEnumerator dash(float d_speed, float d_time)
    {
        while(d_time > dashTimer)
        {
            dashTimer += Time.deltaTime;
            rb.velocity = new Vector2(d_speed, 0);
            //Debug.Log("dashing");
            yield return 0;
        }
        isDashing = false;
    }

    void OnCollisionEnter(Collision collision) //if player hits anything, stop dashing
    {
        dashTimer = dashTime + 1f;
    }
}
