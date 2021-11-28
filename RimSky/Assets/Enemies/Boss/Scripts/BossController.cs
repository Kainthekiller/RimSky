using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{

    public GameObject spawner;
    public float timeForSpawner = 1f;
    Animator animator;

    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Movement")]
    [Space()]
    public NavMeshAgent agent;
    public float timeToRun = 3f;
    private float walkTime = 0f;
    public float walkSpeed = 3.5f;
    public float runSpeed = 6f;

    [Header("Stats")]
    [Space()]
    public float health;
    public int lightDamage = 10, heavyDamage = 45;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public float timeBetweenAttacks;
    public float[] probability;
    private int animSpeed;

    public bool bossStart=false;
    public bool bossDead = false;

    [Header("Trophy")] 
    [Space()] 
    public GameObject Trophy;
    public Transform TrophySpawn;
    private bool fightover = false;

    public BossSwordDamage damage;

    bool alreadyAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        animSpeed = Animator.StringToHash("Speed");        
    }

    void Update()
    {
        if (bossStart && !bossDead)
        {
            playerInSightRange = CheckPlayerRange(sightRange);
            playerInAttackRange = CheckPlayerRange(attackRange);
            if (!playerInSightRange && !playerInAttackRange)
            {
                //Idle();
                ResetRun();
            }
            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            if (playerInSightRange && playerInAttackRange)
            {
                AttackPlayer(); 
                ResetRun();
            }
        }

        if (bossDead && !fightover)
        {
            fightover = true;
            //spawn trophy
            Instantiate(Trophy, TrophySpawn);
        }
    }

    private bool CheckPlayerRange(float range)
    {
        return Physics.CheckSphere(transform.position, range, whatIsPlayer);
    }
    private void Idle()
    {
        agent.SetDestination(transform.position);
    }

    public void BossCleanUp()
    {
        Destroy(this.gameObject);
    }


    private void ChasePlayer()
    {
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
        agent.SetDestination(transform.position);
        animator.SetFloat(animSpeed, 0);
        if (GameManager.Instance.Player.GetComponent<Animator>().GetBool("Grounded"))
        {
            LookAtPlayer();
        }

        if (!alreadyAttacking)
        {
            RandomizeAttack();
        }

    }
    private void LookAtPlayer()
    {
        transform.LookAt(GameManager.Instance.Player.transform.position);
    }
    public void ResetAttack()
    {
        alreadyAttacking = false;
        animator.ResetTrigger("Attack1"); 
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Chant");
    }
    public void ResetRun()
    {
        walkTime = 0f;
    }
    public void StartBoss()
    {
        bossStart = true;
    }
    public void SpawnEnemy()
    {
        spawner.SetActive(true);
        Invoke(nameof(StopSpawnEnemy), timeForSpawner);
    }
    public void StopSpawnEnemy()
    {
        spawner.SetActive(false);
    }

    public void RandomizeAttack()
    {
        //Debug.Log("choosing attack");
        float rand = ((int)Chance(probability));
        if ((int)rand <= probability[0])
        {
           // Debug.Log("choose attack 1");
            animator.SetTrigger("Attack1");
            damage.SetHit(lightDamage);
        }
        else if ((int)rand <= probability[0]+probability[1])
        {
           // Debug.Log("choose attack 2");
            animator.SetTrigger("Attack2");
            damage.SetHeavyHit(heavyDamage); 
        }
        else if ((int)rand <= probability[0] + probability[1]+probability[2])
        {
           // Debug.Log("choose chant");
            animator.SetTrigger("Chant");
        }

        alreadyAttacking = true; 
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
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
    public void TakeDamages(int damage)
    {        
        health -= damage;
        health = Mathf.Clamp(health, 0, 200);
        if (health <= 0)
        {
            agent.SetDestination(transform.position);
            animator.SetBool("isDead", true);
            bossDead = true;
        }
        else
        {
            animator.SetTrigger("takeDamage");
        }
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
