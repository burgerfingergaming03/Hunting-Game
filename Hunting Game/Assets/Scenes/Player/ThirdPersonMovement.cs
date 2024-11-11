using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    //moving
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    //jumping
    public float jumpHeight = 3f;

    //falling
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    Vector3 fallVelocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        //create invisible sphere around GroundCheck and check is collided with ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && fallVelocity.y < 0)
        {
            //keep player on ground;
            fallVelocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //if the user is moving the player
        if(direction.magnitude >= 0.1f)
        {
            //Atan2 returns angle between x axis and a vector (in this case the cam)
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //smooths player turning
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //calculate angle of camera and when player moves auto change direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        //if player on floor and user jumping player
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //jumping physics be like
            fallVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //gravity physics be like
        fallVelocity.y += gravity * Time.deltaTime;
        controller.Move(fallVelocity * Time.deltaTime);
    }
}
