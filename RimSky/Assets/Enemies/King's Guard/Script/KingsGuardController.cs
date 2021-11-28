using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KingsGuardController : MonoBehaviour
{
    private Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public bool isPlayerDead;
   
    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Stats")]
    [Space()]
    int Maxhealth = Mathf.Clamp(150, 0, 150);
    int CurrentEnemyHealth;

    private CapsuleCollider enemyCollider;
    public int _swordDamage;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<CapsuleCollider>();
    }
    void Start()
    {
        
        player = GameManager.Instance.Player.transform;
        animator = gameObject.GetComponent<Animator>();
        CurrentEnemyHealth = Maxhealth;
        alreadyAttacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerRange();
        PlayerDead();
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
        }
       if (playerInSightRange && !playerInAttackRange && isPlayerDead == false)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange && isPlayerDead == false)
        {
            AttackPlayer();
        }
    }

    private void CheckPlayerRange()
    {
        //Check for sight  and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkpoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkpoint.magnitude < 1)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
            SwordDamage sn = gameObject.GetComponent<SwordDamage>();
            sn.SwordBoxColliderDisabled();
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
            animator.SetBool("Attack1", false);
    }


    private void AttackPlayer()
    {
        //Make sure enemy stops moving
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        animator.SetBool("isRunning", false);

            if (!animator.GetBool("Attack1") && isPlayerDead == false)
            {
                //Attack code here
                animator.SetBool("Attack1", true);
                alreadyAttacked = true;
            }
            else
            {
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("Attack1", false);
    }

    public void TakeDamages(int damage)
    {
        
        CurrentEnemyHealth -= damage;
        CurrentEnemyHealth = Mathf.Clamp(CurrentEnemyHealth, 0, 100);
        if (CurrentEnemyHealth <= 0)
        {
            agent.SetDestination(transform.position);
            animator.SetBool("isDead", true);
        }
        else
        {
            
            animator.SetTrigger("isDamaged");
        }

        //animator.SetBool("isDamaged", false);
    }
    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    public void TurnOffBoxColliderHelper()
    {
        enemyCollider.enabled = false;
    }

    public void PlayerDead()
    {
        if (player.GetComponent<Animator>().GetBool("playerDead") == true)
        {
            isPlayerDead = true;
        }
        else
        {
            isPlayerDead = false;
        }
    }
}
