using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewKGController : MonoBehaviour
{
    [Header("Movement")]
    [Space()]
    public UnityEngine.AI.NavMeshAgent agent;
    public float timeToRun = 3f;
    private float walkTime = 0f;
    public float walkSpeed = 3.5f;
    public float runSpeed = 6f;

    [Header("Stats")]
    [Space()]
    private int Maxhealth = Mathf.Clamp(100, 0, 100);
    public int CurrentEnemyHealth;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public float timeBetweenAttacks;
    public float[] probability;
    private int animSpeed;
    public int _swordDamage;
    private Animator animator;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public bool isPlayerDead;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    bool alreadyAttacking;

    private CapsuleCollider enemyCollider;

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyCollider = GetComponent<CapsuleCollider>();
    }
    void Start()
    {

        player = GameManager.Instance.Player.transform;
        animator = gameObject.GetComponent<Animator>();
        CurrentEnemyHealth = Maxhealth;
        animSpeed = Animator.StringToHash("Speed");
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = CheckPlayerRange(sightRange);
        playerInAttackRange = CheckPlayerRange(attackRange);

        if (!playerInSightRange && !playerInAttackRange)
        {
            //Idle();
            ResetRun();
            Patrolling();
        }
        if (playerInSightRange && !playerInAttackRange && animator.GetBool("isDead") == false)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange && animator.GetBool("isDead") == false)
        {
            AttackPlayer();
            ResetRun();
        }
    }

    private bool CheckPlayerRange(float range)
    {
        return Physics.CheckSphere(transform.position, range, whatIsPlayer);
    }

    private void Idle()
    {
        agent.SetDestination(transform.position);
        animator.SetFloat(animSpeed, 0);
    }

    public void ResetRun()
    {
        walkTime = 0f;
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
        //SwordDamage sn = gameObject.GetComponent<SwordDamage>();
        //sn.SwordBoxColliderDisabled();
        agent.SetDestination(GameManager.Instance.Player.transform.position);
        walkTime += Time.deltaTime;
        animator.SetFloat(animSpeed, walkSpeed);
        if (walkTime > timeToRun)
        {
            animator.SetFloat(animSpeed, runSpeed);
        }
        else
        {
            animator.SetFloat(animSpeed, walkSpeed);
        }
    }

    private void AttackPlayer()
    {
        //Make sure enemy stops moving
        agent.SetDestination(transform.position);
        animator.SetFloat(animSpeed, 0);
        if (GameManager.Instance.Player.GetComponent<Animator>().GetBool("Grounded"))
        {
            transform.LookAt(GameManager.Instance.Player.transform.position);
        }

        if (!alreadyAttacking)
        {
            RandomizeAttack();
        }
    }

    private void ResetAttack()
    {
        alreadyAttacking = false;
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
    }

    public void RandomizeAttack()
    {
        //Debug.Log("choosing attack");
        float rand = ((int)Chance(probability));
        if ((int)rand <= probability[0])
        {
            // Debug.Log("choose attack 1");
            animator.SetTrigger("Attack1");
        }
        else if ((int)rand <= probability[0] + probability[1])
        {
            // Debug.Log("choose attack 2");
            animator.SetTrigger("Attack2");
        }

        alreadyAttacking = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    public void TakeDamages(int damage)
    {
        CurrentEnemyHealth -= damage;
        CurrentEnemyHealth = Mathf.Clamp(CurrentEnemyHealth, 0, 100);
        if (CurrentEnemyHealth <= 0)
        {
            agent.SetDestination(transform.position);
            animator.SetBool("isDead", true);
            animator.SetFloat(animSpeed, 0);
            TurnOffBoxColliderHelper();
        }
        else
        {
            animator.SetTrigger("isDamaged");
        }
    }
    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    public void TurnOffBoxColliderHelper()
    {
        enemyCollider.enabled = false;
    }

    float Chance(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
