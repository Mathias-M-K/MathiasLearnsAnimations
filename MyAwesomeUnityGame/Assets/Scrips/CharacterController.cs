using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Animations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour
{
    private Animator _animationController;

    #region properties (public)

    [Header("Character Status")] 
    public float distanceToGround;
    public bool isGrounded;
    public bool breakFall;

    [Header("Character Walking")] 
    public float walkAcceleration = 0.5f;
    public float slowDownSmoothingFactor = 20;
    public float maxWalkSpeed = 10;

    [Header("Character Jumping")] 
    public float jumpForce = 300;
    public float jumpDelay = 0.25f;
    public ForceMode forceMode = ForceMode.Force;
    public float airSpeed;
    public float distanceBeforeGrounded = 0.05f;
    public float distanceBeforeBreakFall = 0.5f;

    [Header("Prototyping")] 
    public float gravity = 600;
    public float gravityMultiplier = 1;
    public float fallMultiplier = 2.5f;
    public float rayOffset = 0.4f;

    [Header("Animation Settings")] 
    public KeyCode jumpKey;
    public KeyCode walkRight;
    public KeyCode walkLeft;

    [Header("Animation Feedback")] 
    public JumpingObserver jumpObs;

    #endregion

    #region Movement Values

    private float _velZ, _velY;

    #endregion

    private Rigidbody _rb;
    private bool _jumpInitiated;

    // Start is called before the first frame update
    void Start()
    {
        jumpObs.cc = this;

        _animationController = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if character is grounded
        CheckIfGrounded();

        //Updating animator
        _animationController.SetBool("IsGrounded", isGrounded);

        //If the jumpInitiated parameter have been set to true, it will be set to false when character have cleared the ground
        if (distanceToGround > distanceBeforeBreakFall) _jumpInitiated = false;

        //If jumping, breakFall is forced false, until the character have cleared the ground above the breakFall parameter
        if (_jumpInitiated)
        {
            breakFall = false;
        }
        else
        {
            breakFall = distanceToGround < distanceBeforeBreakFall;
            _animationController.SetBool("BreakFall", breakFall);
        }


        /*
         * JUMP
         */
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            _animationController.SetBool("Jump", true);

            //Setting jump variables
            _jumpInitiated = true;
            breakFall = false;

            //Setting breakFall variable
            _animationController.SetBool("BreakFall", breakFall);

            //Initiating the jump
            StartCoroutine(Jump(jumpForce));
        }


        /*
         * WALK
         */

        if (Input.GetKey(walkRight) || Input.GetKey(walkLeft))
        {
            _animationController.SetBool("Walk", true);
            
            if (Input.GetKey(walkRight))
            {
                _velZ += -walkAcceleration * Time.deltaTime;
                //_velZ = -maxWalkSpeed;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetKey(walkLeft))
            {
                _velZ += walkAcceleration * Time.deltaTime;
                //_velZ = maxWalkSpeed;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }


            if (Math.Abs(_velZ) > maxWalkSpeed)
            {
                _velZ = _velZ > 0 ? maxWalkSpeed : -maxWalkSpeed;
            }
        }
        else
        {
            _animationController.SetBool("Walk", false);
            _velZ = Mathf.SmoothStep(_velZ, 0, Time.deltaTime * slowDownSmoothingFactor);
        }

        /*
         * Gravity
         */
        if (!isGrounded)
        {
            if (_rb.velocity.y < 0)
            {
                gravityMultiplier = fallMultiplier;
            }
            else
            {
                gravityMultiplier = 1;
            }


            _rb.AddForce(Vector3.down * gravity * gravityMultiplier);
            
            if (Math.Abs(_velZ) > airSpeed)
            {
                _velZ = _velZ > 0 ? airSpeed : -airSpeed;
            }
        }


        _rb.velocity = new Vector3(0, _rb.velocity.y, _velZ);
        _animationController.SetFloat("Speed", GlobalMethods.Remap(math.abs(_rb.velocity.z), 0, maxWalkSpeed, 0, 1));
    }

    private void SetGravityMultiplier(float newMultiplier)
    {
        gravityMultiplier = newMultiplier;
    }

    private void CheckIfGrounded()
    {
        //Checking if character is grounded
        Vector3 currentPos = transform.position;
        Vector3 rayOffsetPosLeft = new Vector3(currentPos.x, currentPos.y, currentPos.z + rayOffset);
        Vector3 rayOffsetPosRight = new Vector3(currentPos.x, currentPos.y, currentPos.z - rayOffset);

        Physics.Raycast(rayOffsetPosLeft, Vector3.down, out RaycastHit hit1, 100);
        Physics.Raycast(rayOffsetPosRight, Vector3.down, out RaycastHit hit2, 100);

        distanceToGround = hit1.distance > hit2.distance ? hit2.distance : hit1.distance;


        //Drawing Ray
        /*
            Debug.DrawRay(rayOffsetPosLeft, Vector3.down, Color.red,100);
            
            Debug.DrawRay(rayOffsetPosRight, Vector3.down, Color.green,100);
            */

        isGrounded = distanceToGround < distanceBeforeGrounded;
    }

    IEnumerator Jump(float forceJump)
    {
        yield return new WaitForSeconds(jumpDelay);
        _rb.AddForce(Vector3.up * forceJump, forceMode);
    }

    /// <summary>
    /// Method called externally from animation state-machine. Indicates that the jumping animation is being played.
    /// </summary>
    /// <param name="animationJumpState"></param>
    public void SetAnimationJumpState(bool animationJumpState)
    {
        _animationController.SetBool("Jump", animationJumpState);
    }
}