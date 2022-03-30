using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Objects
    GameObject player = null;
    Camera cam = null;
    CharacterController charcontroller = null;
    GameObject pausemenu = null;
    Image crosshair = null;
    //AudioSource walkAudio;

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
    float yaw;
    float pitch;
    bool showCursor = false;

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
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        pausemenu = GameObject.Find("PauseMenu");
        pausemenu.SetActive(false);
        
        //walkAudio = player.GetComponent<AudioSource>();
        movementdirection = Vector3.zero;

        //Set the user name to the one the user has assigned in the settings page
        SetUserName();

        //Lock the cursor
        ShowCursor(showCursor);
    }

    private void Update()
    {
        if (!Settings.gamePaused)
        {
            CheckKeyBoardInputs();
            CheckMouseInputs();
        }

        //Pause game and show menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Settings.PauseGame(!Settings.gamePaused);
            ToggleCursor();
            ShowCursor(showCursor);
            pausemenu.SetActive(!pausemenu.activeSelf);
            crosshair.gameObject.SetActive(!crosshair.gameObject.activeSelf);
        }
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

        //Work on this later
  /*      if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (!walkAudio.isPlaying)
            {
                walkAudio.Play();
                print("walkaudio.play");
            }

            if (!charcontroller.isGrounded)
            {
                walkAudio.Stop();
            }
        }
        else
        {
            walkAudio.Stop();
        }*/

        //Gravity's pull means that the vertical jump speed will decrease, and go into the negative, pulling the player back to the surface
        verticalSpeed -= gravity * Time.deltaTime;


        //MAKE MOVEMENT VECTOR

        //Bind vector to 3 directions, forward, up and right. These can be negavite or positive, giving us the directions we need to move the player with
        //Do note that if movement speed is multiplied by sprinting, the player will also jump higher with this code
        movementdirection = (transform.forward * Input.GetAxis("Vertical") + transform.up * verticalSpeed + transform.right * Input.GetAxis("Horizontal"));

        //Move the player with the provided vector, in time.deltatime ofcourse
        charcontroller.Move(movementdirection * walkspeed * Time.deltaTime);
    }

    void CheckMouseInputs()
    {
        yaw += Settings.mouseSens * Input.GetAxis("Mouse X") / 100; //Left and right
        pitch -= Settings.mouseSens * Input.GetAxis("Mouse Y") / 100; //Up and down

        //Clamp the pitch so that we cant go further than -90 or 90 degrees up or down
        pitch = Mathf.Clamp(pitch, -90, 90);

        charcontroller.transform.eulerAngles = new Vector3(0f, yaw, 0f);
        cam.transform.eulerAngles = new Vector3(pitch, yaw, 0);
    }

    void SetUserName()
    {
        if (TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            humanoid.username = Settings.username;
        }
    }

    public void ShowCursor(bool boolean)
    {
        Cursor.visible = boolean;

        if (boolean == true)
        {
            if (Settings.keepCursorInApplicationWindow)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ToggleCursor()
    {
        showCursor = !showCursor;
    }
}
