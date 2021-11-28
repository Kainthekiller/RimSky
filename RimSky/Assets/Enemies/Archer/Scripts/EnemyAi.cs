using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    private Animator animator;
    
    [Header("Movement")]
    [Space()]
    public GameObject projectile;
    public NavMeshAgent agent;
    public LayerMask WhatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    public float walkPointRange;

    bool walkPointSet;
    
    [Header("Stats")]
    [Space()]
    public float health;
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Attacking")]
    [Space()]
    //Attacking
    public Transform _fireLocation;
    public float timeBetweenAttacks;
    public float _arrowSpeed;
    public float _arrowArcForce;
    public int _arrowDamage;

    [Header("Credit Tools")]
    [Space()]
    public bool CreditMode = false; //Enable this to set a permanent target
    public bool RunMode = false;    //enable this to run to a set point
    public Transform RunPoint;

    public Transform PermaAttack;
    bool alreadyAttacking;

    private CapsuleCollider enemyBoxCollider;

    private void Awake()
    {
        //Grabs the transform location 
        
        agent = GetComponent<NavMeshAgent>();
        enemyBoxCollider = GetComponent<CapsuleCollider>();
    }
    private void Start()
    {

        animator = this.gameObject.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckPlayerRange();
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
         
        }
        if (playerInSightRange && playerInAttackRange )
        {
            if (!alreadyAttacking)
            {
             
            }
            AttackPlayer();
        }
        if (CreditMode)
        {
            if (!alreadyAttacking)
            {

            }
            AttackPlayer();
        }
        if (RunMode)
        {
            ChasePlayer();
        }
    }

    private void CheckPlayerRange()
    {

        //Check for sight  and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround))
        {
            walkPointSet = true;
        }

    }

    private void Patrolling()
    {
        animator.SetBool("isWalking", false);
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walk point reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }


    }
    private void ChasePlayer()
    {
        if (RunMode)
        {
            agent.SetDestination(RunPoint.position);
            animator.SetBool("isWalking", true);
            animator.SetBool("shootArrowInPlace", false);
        }
        else
        {
            agent.SetDestination(GameManager.Instance.Player.transform.position);
            animator.SetBool("isWalking", true);
            animator.SetBool("shootArrowInPlace", false);
        }
        
        
    }
    public void ShootArrow()
    {
            // Attack Code for Projectile HERE
            Rigidbody rb = Instantiate(projectile, _fireLocation.position, transform.rotation).GetComponent<Rigidbody>();

            rb.gameObject.GetComponent<ArrowDamage>().SetDamage(_arrowDamage);

            rb.AddForce(transform.forward * _arrowSpeed, ForceMode.Impulse);
            rb.AddForce(transform.up * _arrowArcForce, ForceMode.Impulse);

            //


            alreadyAttacking = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        
    }
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move

        if (CreditMode)
        {
            //agent.SetDestination(PermaAttack.position);
            animator.SetBool("isWalking", false);
            animator.SetBool("shootArrowInPlace", true);
            transform.LookAt(PermaAttack.transform.position);

        }
        else
        {
            agent.SetDestination(GameManager.Instance.Player.transform.position);
            animator.SetBool("isWalking", false);
            animator.SetBool("shootArrowInPlace", true);
            if (GameManager.Instance.Player.GetComponent<Animator>().GetBool("Grounded"))
            {
                transform.LookAt(GameManager.Instance.Player.transform.position);
            }
            
        }

            //if (!alreadyAttacking)
            //{
            //    // Attack Code for Projectile HERE
            //    Rigidbody rb = Instantiate(projectile, transform.position + new Vector3(0.0f,1f,0.0f), transform.rotation).GetComponent<Rigidbody>();

            //    rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
            //    rb.AddForce(transform.up * 3f, ForceMode.Impulse);

            //    //


            //    alreadyAttacking = true;
            //    Invoke(nameof(ResetAttack), timeBetweenAttacks);
            //}


    }

    private void ResetAttack()
    {
        alreadyAttacking = false;
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public void TurnOfBoxColliderHelper()
    {
        enemyBoxCollider.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, attackRange);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, sightRange);
    }




}



