using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    Camera playerCamera;
    [SerializeField] float runSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] Transform raycastTransform;
    Vector3 runForwardDirection;
    Vector3 runSidewaysDirection;
    Vector3 newDirection;

    [Header("Right Click Rotate Camera")]
    [SerializeField] float speedHorizontal = 2.0f;
    [SerializeField] float speedVertical = 2.0f;
    float yaw = 0f;
    float pitch = 0f;

    [SerializeField] float jumpPower;
    [SerializeField] float jumpVelocity;
    [SerializeField] float gravity = 5;

    Animator animController;
    Collider collider;
    Rigidbody rb;
    RaycastHit hit;
    public bool isGrounded;
    void Start()
    {
        playerCamera = FindObjectOfType<Camera>();
        animController = GetComponentInChildren<Animator>();
        collider = GetComponentInChildren<Collider>();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInput();
        if (CheckForGrounded())
            Jump();
        Gravity();
    }

    public void HandlePlayerInput(Vector3 velocity)
    {
        if (isGrounded)
        {
            // A raycast to determine the surface normal of the object below us
            Physics.Raycast(transform.position, Vector3.down, out hit);

            // The angle between the surface normal of the object below us and a vector straight to the sky
            float angle = Vector3.Angle(hit.normal, Vector3.up);

            // If its flat, the angle will always be 0, if it is elevated, we will set it to a new velocity vector
            if (angle != 0)
            {
                // Set the movement vectors Y value to the normalized cross product Y (this will make us move fluidly up slopes)
                velocity = new Vector3(velocity.x, Vector3.Cross(-transform.right, hit.normal).normalized.y, velocity.z);
                velocity *= runSpeed;
            }
            else
                velocity *= runSpeed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
    }
    void GetUserInput()
    {
        /*
        // Character Doesnt Rotate while Right click is down unless they use the mouse to turn, a and d become strafe keys
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && Input.GetMouseButton(1))
        {
            //CalculateRunMovementRightClickDown();
        }
        else*/ if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            CalculateMovementRightClickUp();
        }
        /*else if (Input.GetMouseButton(1))
        {
            //RotateCamera();
            runForwardDirection = Vector3.zero;
            runSidewaysDirection = Vector3.zero;
            newDirection = Vector3.zero;
        }*/
        else
        {
            animController.SetBool("running", false);
            animController.SetBool("walkingBackwards", false);
            runForwardDirection = Vector3.zero;
            runSidewaysDirection = Vector3.zero;
            newDirection = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            transform.position += transform.up * .2f;
            rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            animController.SetBool("startedJump", true);
            StartCoroutine(AnimationBoolDelay("startedJump", false, 0.2f));
        }
    }

    void CalculateRunMovementRightClickDown()
    {
        newDirection = Vector3.zero;
        runForwardDirection = Vector3.zero;
        runSidewaysDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            runForwardDirection = Vector3.forward;
        else if (Input.GetKey(KeyCode.S))
            runForwardDirection = Vector3.back / 2;
        if (Input.GetKey(KeyCode.D))
            runSidewaysDirection = Vector3.right;
        else if (Input.GetKey(KeyCode.A))
            runSidewaysDirection = Vector3.left;
        newDirection = ((runSidewaysDirection + runForwardDirection).normalized * runSpeed * Time.deltaTime);
        //RotateCamera();
        transform.Translate(newDirection);
    }
    void CalculateMovementRightClickUp()
    {
        RaycastHit raycastHit;
        if (Input.GetKey(KeyCode.W))
        {
            animController.SetBool("walkingBackwards", false);
            Physics.Raycast(raycastTransform.position, transform.forward, out raycastHit, .5f, layerMask);

            if (raycastHit.collider == null)
                runForwardDirection = Vector3.forward;
            else
                runForwardDirection = Vector3.zero;
            animController.SetBool("running", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animController.SetBool("running", false);
            Physics.Raycast(raycastTransform.position, -transform.forward, out raycastHit, .2f, layerMask);
            if (raycastHit.collider == null)
                runForwardDirection = Vector3.back / 2;
            animController.SetBool("walkingBackwards", true);
        }
        else
        {
            animController.SetBool("running", false);
            animController.SetBool("walkingBackwards", false);
            runForwardDirection = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(0, turnSpeed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            // transform.eulerAngles += new Vector3(0, -turnSpeed * Time.deltaTime, 0);
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
        }
        transform.Translate(runForwardDirection * runSpeed * Time.deltaTime);
    }
    void RotateCamera()
    {
        yaw += speedHorizontal * Input.GetAxis("Mouse X");
        pitch += speedVertical * Input.GetAxis("Mouse Y");
        Vector3 previousPosition = playerCamera.transform.position;
        playerCamera.transform.position = transform.position;
        playerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0);
        playerCamera.transform.Translate(previousPosition );
    }
    void Gravity()
    {
        if (jumpVelocity > 0)
        {
            jumpVelocity -= gravity * Time.deltaTime;
        }
        else
            jumpVelocity = 0;

        transform.position += (Vector3.down * gravity + Vector3.up * jumpVelocity) * Time.deltaTime;
        if (transform.position.y > 0)
        {
            transform.position += (Vector3.down * gravity + Vector3.up * jumpVelocity) * Time.deltaTime;
            if (transform.position.y < 0.2f)
            {
                animController.SetBool("endedJump", true);
            }
        }
        else if (transform.position.y <= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

            animController.SetBool("endedJump", false);
        }
    }
    void Jump()
    {
        if (jumpVelocity > 0)
        {
            jumpVelocity -= gravity * Time.deltaTime;
        }
        else
            jumpVelocity = 0;
    }

    //// Returns true if player is moving, false if they are not.
    //public bool IsPlayerMoving()
    //{
    //    if (rb.velocity != Vector3.zero)
    //        return true;
    //    return false;
    //}
    bool CheckForGrounded()
    {
        float extraHeight = .1f;
        RaycastHit raycastHit;
        Physics.Raycast(collider.bounds.center, Vector3.down, out raycastHit, collider.bounds.extents.y + extraHeight, layerMask);
        if (raycastHit.collider != null)
        {
            animController.SetBool("endedJump", true);
            isGrounded = true;
            return true;
        }
        else
        {
            animController.SetBool("endedJump", false);
            isGrounded = false; 
            return false;
        }
            
    }
    IEnumerator AnimationBoolDelay(string setBoolName, bool stateToEnter, float timeToDelay)
    {
        yield return new WaitForSeconds(timeToDelay);
        animController.SetBool(setBoolName, stateToEnter);
    }
    IEnumerator SlightJumpDelay()
    {
        yield return new WaitForSeconds(0.2f);

    }
}
