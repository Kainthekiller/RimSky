using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
public class PlayerController : MonoBehaviour
{

    //Properties
   
    float maxDelayComboTime = 1f;
    float trackTime;
    Mouse mouse = Mouse.current; //Mouse Input 
    Keyboard keyboard = Keyboard.current; //Keyboard Input
    int attackCounter = 0;
    Vector2 moveDirection;
    float jumpDirection;
    float rollDirection;
    public float moveSpeed = 2;
    public float maxForwardSpeed = 8;
    public float turnSpeed = 30f;
    float desiredSpeed;
    float forwardSpeed;
    float jumpSpeed = 25000f;
    const float groundAccelration = 5;
    const float groundDecel = 25;
    Animator animator;
    Rigidbody rb;
    [SerializeField] int Maxhealth = Mathf.Clamp(100,0,100);
    [SerializeField] int MaxMana = Mathf.Clamp(100, 0, 100);
    int CurrentPlayerHealth;
    int CurrentPlayerMana;
    float StrongAttackCharge;
    public HealthBar healthbar;
    public ManaBar manaBar;
    private float CooldownTimer = 0;
    private float CooldownResetTime = 3;
    public GameObject FireballGameObject;
    public Transform FireballSpawnPoint;
    public AudioSource audiosource;
    
    //Properties

    //Checks if input to see if the player is moving 
    bool IsMoveInput
    {
        get { return !Mathf.Approximately(moveDirection.sqrMagnitude, 0f); }
    }

    //Uses Player Input Package
    public void OnMove(InputAction.CallbackContext context)
    //X   //Y     //X   //Y      //X   //Y       //X   //Y 
    {   //Usually (1.0, 0.0) or (0.0, 1.0) or (-1.0 , 0.0) or (0.0, -1.0)
        moveDirection = context.ReadValue<Vector2>();
        // Debug.Log(moveDirection);
    }
    //public void OnAttack(InputAction.CallbackContext context)
    //{
    //    attackRate = context.ReadValue<float>();
    //  //  Debug.Log(attackRate);
    //}

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpDirection = context.ReadValue<float>();
    }
    public void OnRoll(InputAction.CallbackContext context)
    {
        rollDirection = context.ReadValue<float>();
    }
    //Ref in the animation Attack 1
    void timeTracker()
    {
        trackTime = Time.time;
    }
    //Hold Mouse 2 to block
    public void Blocking()
    {
        if (mouse.rightButton.isPressed)
        {
            animator.SetBool("isBlocking", true);
        }
        else
        {
            animator.SetBool("isBlocking", false);
        }
        
    }
    
    //Main Combo Attack 1 2 3
    public void Attack()
    {
        //Records 1 Mouse 0 Click until released
        if (mouse.leftButton.wasPressedThisFrame)
        {
            timeTracker();
            animator.SetBool("Attack1", true);
            animator.SetBool("Attack1Delay", true);
        }
        else
        {
            animator.SetBool("Attack1", false);
        }
        if (attackCounter == 2)
        {
            animator.SetBool("Attack2", true);
        }
        if(attackCounter == 3)
        {
            animator.SetBool("Attack3", true);
        }

        if (mouse.leftButton.wasPressedThisFrame)
        {
            attackCounter++;
           // Debug.Log("Code " + attackCounter);
        }
        if (Time.time - trackTime > maxDelayComboTime)
        {
            attackCounter = 0;
            animator.SetBool("Attack1Delay", false);
            animator.SetBool("Attack2", false);
            animator.SetBool("Attack3", false);
        }
    }
    //Jump Attack Function
    public void JumpAttack()
    {
        if (mouse.leftButton.wasPressedThisFrame)
        {
            animator.SetBool("jumpAttack", true);
        }
        else
        {
            animator.SetBool("jumpAttack", false);
        }
    }

    //Called in update using moveDirections from OnMove Function
    void Move(Vector2 direction)
    {
       

        float turnAmount = direction.x;
        float fDirection = direction.y;
        if (direction.sqrMagnitude > 1f)
        {
            direction.Normalize();
        }
        // X^2 + Y^2 =  Z^2                                         //Returns 1 or -1 depending if positive number or negative # 
        desiredSpeed = direction.magnitude * maxForwardSpeed * Mathf.Sign(fDirection);


        //True Speed up Rate    False Slow Down Rate
        float acceleration = IsMoveInput ? groundAccelration : groundDecel;
        //Current Speed  //Max Speed     //How fast 
        forwardSpeed = Mathf.MoveTowards(forwardSpeed, desiredSpeed, acceleration * Time.deltaTime);
        //Blend Tree in the animator -10 is back words and +10 is forward  
        animator.SetFloat("ForwardSpeed", forwardSpeed);

        //TurnAround               Uses X Direction which is either 1 or -1 * turn speed to rotate charcter.
        if (turnAmount == -1)
        {
            turnAmount = -1f;
        }
        else if (turnAmount == 1)
        {
            turnAmount = 1f;
        }
        else
        {
            turnAmount = direction.x;
        }

        transform.Rotate(0, turnAmount * 50f * Time.deltaTime, 0);
        //Debug.Log(turnAmount);
        //Helps when you jump so you can kinda go left and right if you want.
        if (direction.x == -1.0)
        {
            rb.AddForce(10f * -transform.right);
            //rb.AddRelativeForce(Vector3.left * 200f);
            //  Debug.Log("force added");
        }
        else if (direction.x == 1.0)
        {
            rb.AddRelativeForce(Vector3.right * 200f);
            animator.SetBool("readyRightRoll", true);
            // Debug.Log("force added");
        }
        else
        {
            animator.SetBool("readyRightRoll", false);
        }

    }


    // Jump Method Attached to Animation to switch allowing for a jump or not
    void Jump(float direction)
    {
        if (direction != 0)
        {
            animator.SetBool("readyJump", true);

        }
        else if (direction == 0)
        {
            animator.SetBool("readyJump", false);
        }

    }
    //For Ground Rolling with Shift Key
    void Roll(float direction)
    {
       // Debug.Log(direction);
        if (direction != 0)
        {
            animator.SetBool("readyRoll", true);
 

        }
        else if (direction == 0)
        {
            animator.SetBool("readyRoll", false);

        }

    }

    //For Regular Jumping not forward or backwards
    public void Launch()
    {
        rb.AddForce(0, jumpSpeed, 0);
        rb.AddRelativeForce(Vector3.forward * 10000f);
        animator.applyRootMotion = false;
    }

    //Used When Back Jumping so the player goes back
    public void BackForce()
    {
        rb.AddForce(0, jumpSpeed - 10000f, 0);
        rb.AddRelativeForce(Vector3.back * 10000f);
        animator.applyRootMotion = false;
    }

    //For Running Jumping then Roll Force
    public void RollingForce()
    {
        rb.AddForce(0, jumpSpeed - 9000f, 0);
        rb.AddRelativeForce(Vector3.forward * 25000f);
        animator.applyRootMotion = false;
    }

    public void FireBallAttack()
    {
        CurrentPlayerMana = manaBar.GetMana();
        CurrentPlayerMana -= 20;
        CurrentPlayerMana = Mathf.Clamp(CurrentPlayerMana, 0, 100);
        manaBar.SetMana(CurrentPlayerMana);
        // need to figure out how to delay this until the animator is fully extended for his arm
        //maybe add a fireball delay function that counts down from like 0.3 ish and fine tune from there.

        CooldownTimer = CooldownResetTime;
        GameObject b = Instantiate(FireballGameObject, FireballSpawnPoint.position + (transform.forward / 2), transform.rotation) as GameObject;
        // Debug.Log("Finger: "+FireballSpawnPoint.position.x + " " + FireballSpawnPoint.position.y + " " + FireballSpawnPoint.position.z + " ");
        // b.GetComponent<Rigidbody>().velocity = transform.forward * FireballSpeed;
       
    }

    public void StrongAttack()
    {
        if (keyboard.eKey.wasPressedThisFrame)
        {
            animator.SetTrigger("StrongAttack");
        }
    }


    //For Ground Rolling with Shift Key
    public void GroundRoll()
    {
        rb.AddRelativeForce(Vector3.left * 10000f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Turns on after landing on the ground
        animator.applyRootMotion = true;
        //Enemys take damage in SwordScript;
    }

    public void TakeDamage(int damage)
    {
        CurrentPlayerHealth = healthbar.GetHealth();
        //Trigger 
        animator.SetTrigger("takeDamage");
        CurrentPlayerHealth -= damage;
        CurrentPlayerHealth = Mathf.Clamp(CurrentPlayerHealth, 0, 100);
        healthbar.SetHealth(CurrentPlayerHealth);
    }
    public void FireballTriggerAttack()
    {
        CurrentPlayerMana = manaBar.GetMana();
        if (keyboard.fKey.wasPressedThisFrame && CurrentPlayerMana != 0)
        {
            animator.SetTrigger("fireballAttack");
        }

    }

    public void playerDead()
    {
        if (CurrentPlayerHealth == 0)
        {
            animator.SetBool("playerDead", true);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        CurrentPlayerHealth = Maxhealth;
        healthbar.SetMaxHealth(Maxhealth);
        audiosource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        CurrentPlayerMana = MaxMana;
        manaBar.SetMaxMana(MaxMana);
        

    }

    // Update is called once per frame
    void Update()
    {
        Move(moveDirection);
            Roll(rollDirection);
            Attack(); // Set too 2 sec
            Blocking();
            JumpAttack();
            StrongAttack();
            playerDead();
            Jump(jumpDirection);
            FireballTriggerAttack();
        
        

    }
}
