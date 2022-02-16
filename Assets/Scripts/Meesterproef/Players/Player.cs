using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Objects
    GameObject player = null;
    Camera cam = null;
    CharacterController charcontroller = null;

    //Movement & Speed
    [SerializeField] float walkspeed = 4;
    Vector3 movementdirection;

    //Gravity
    float gravity = 9.81f / 2f;
    [SerializeField] float jumpforce = 2;
    float verticalSpeed;

    [Space]
    //Mouse
    [Range(10, 100)]
    public float mouseSens = 75; //Turned this public so that flyingcam can copy its sens
    float yaw;
    float pitch;

    //Jumping
    [Header("Jumping options")]
    [SerializeField] bool allowJumping = true;
    [SerializeField] bool keepJumping = false;

    private void Start()
    {
        //Assign variables
        player = gameObject;
        cam = player.GetComponentInChildren<Camera>();
        charcontroller = player.GetComponent<CharacterController>();
        movementdirection = Vector3.zero;

        //Make cursor locked and invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

        private void Update()
    {
        CheckKeyBoardInputs();
        CheckMouseInputs();
    }

    void CheckKeyBoardInputs()
    {
        //JUMP
        if (charcontroller.isGrounded && allowJumping) //Check if player is grounded before allowing a jump
        {
            verticalSpeed = -1;

            if (Input.GetKey(KeyCode.Space) && keepJumping || Input.GetKeyDown(KeyCode.Space) && !keepJumping)
            {
                verticalSpeed = jumpforce; //Make the player "jump"
            }
        }

        //Gravity's pull means that the vertical jump speed will decrease, and go into the negative, pulling the player back to the surface
        verticalSpeed -= gravity * Time.deltaTime;


        //MAKE MOVEMENT VECTOR

        //Bind vector to 3 directions, forward, up and right. These can be negavite or positive, giving us the directions we need to move the player with
        //Do note that if movement speed is multiplied by sprinting, the player will also jump higher with this code
        movementdirection = (transform.forward * Input.GetAxis("Vertical") + transform.up * verticalSpeed + transform.right * Input.GetAxis("Horizontal"));

        //Move the player with the provided vector, in time.deltatime ofcourse
        charcontroller.Move(movementdirection * walkspeed * Time.deltaTime);


        //Quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void CheckMouseInputs()
    {
        yaw += mouseSens * Input.GetAxis("Mouse X") / 100; //Left and right
        pitch -= mouseSens * Input.GetAxis("Mouse Y") / 100; //Up and down

        //Clamp the pitch so that we cant go further than -90 or 90 degrees up or down
        pitch = Mathf.Clamp(pitch, -90, 90);

        charcontroller.transform.eulerAngles = new Vector3(0f, yaw, 0f);
        cam.transform.eulerAngles = new Vector3(pitch, yaw, 0);
        
    }
}
