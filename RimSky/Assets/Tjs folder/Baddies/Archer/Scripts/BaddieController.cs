using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BaddieState
{
    Idle,
    Chasing,
    Aiming,
    Shooting,
    Hurt,
    Die
}

[System.Serializable]
public struct BaddieStats
{
    public float idleSpeed;
    public float chaseSpeed;
    public float turnSpeed;
    public float aimTime;
    public float fireStrength;
    public float cooldownTime;
    public float hurtTime;
    public int health;
}



public class BaddieController : MonoBehaviour
{
    public Material mat;
    [Header("Have Spawner tell me my target (doesnt have to be player)")]
    private GameObject player;

    [Space()]
    [Header("Leave emty to find automatically")]
    public UnityEngine.AI.NavMeshAgent agent;

    [Space()]
    [Header("Animator for me to update")]
    [SerializeField]
    public Animator myAnimator;

    [Space()]
    [Header("sight area")]
    [SerializeField]
    public Collider mySight;

    [Space()]
    [Header("Current state this enemy is in (dont change manually, will add buttons for that)")]
    [SerializeField]
    private BaddieState currState;


    [Space()]
    [Header("Speed and health for the enemy")]
    [SerializeField]
    private BaddieStats stats;

    [Space()]
    [Header("Projectile")]
    public GameObject Arrow;
    [Space()]
    [Header("Arrow space")]
    public GameObject ArrowLoadSpot;

    private List<Projectile> Arrows;

    private bool canSeePlayer;
    void Start()
    {
        player = GameManager.Instance.Player;
       

        if (agent == null)
            agent = gameObject.GetComponent<NavMeshAgent>();

        if (myAnimator == null)
            myAnimator = gameObject.GetComponent<Animator>();

        currState = BaddieState.Idle;
        
        Arrows = new List<Projectile>();
        for (int i = 0; i < 10; i++)
        {
            Arrows.Add(GameObject.Instantiate(Arrow, ArrowLoadSpot.transform.position, ArrowLoadSpot.transform.rotation, ArrowLoadSpot.transform).GetComponent< Projectile>());
        }
        foreach (Projectile p in Arrows)
        {
            p.rb.constraints = RigidbodyConstraints.FreezeAll;
            p.gameObject.SetActive(false);
        }
    }
   

    void Update()
    {

        switch (currState)
        {
            case BaddieState.Idle:
                ResetAnimator();
                if (agent != null )
                {
                    agent.SetDestination(transform.position);
                }
                break;
            case BaddieState.Chasing:
                myAnimator.SetBool("Chasing", true);
                if (agent != null && player != null)
                {
                    agent.SetDestination(player.transform.position);
                    if (!mySight.bounds.Contains(player.transform.position) || !canSeePlayer)
                    {
                        ReturnToIdle();
                    }
                }
                break;
            case BaddieState.Aiming:
                myAnimator.SetBool("Aiming", true);
                if (agent != null)
                {
                    Vector3 _dir = player.transform.position - transform.position;
                    _dir.Normalize();
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir), stats.turnSpeed * Time.deltaTime);
                    
                    
                    agent.SetDestination(transform.position);
                }
                break;
            case BaddieState.Shooting:
                myAnimator.SetBool("Shooting", true);
                if (agent != null)
                {
                    agent.SetDestination(transform.position);
                }
                break;
            case BaddieState.Hurt:
                myAnimator.SetBool("Hurt", true);
                if (agent != null)
                {
                    agent.SetDestination(transform.position);
                }
                break;
            case BaddieState.Die:
                Destroy(this.GetComponent<CapsuleCollider>());
                myAnimator.SetBool("Die", true);
                break;
            default:
                break;
        } 


    }

    public void ResetAnimator()
    {
        myAnimator.SetBool("Chasing", false); 
        myAnimator.SetBool("Aiming", false);
        myAnimator.SetBool("Shooting", false);
        myAnimator.SetBool("Hurt", false);
    }

    public void EnterSight(Collider collider)
    {
        canSeePlayer = true;
        if (currState == BaddieState.Idle)
                ChasePlayer();
    }

    public void PlayerInSight(Collider collider)
    {

        canSeePlayer = true;
        if (currState == BaddieState.Idle)
        {
            if (collider.gameObject == player)
            {
                EnterSight(collider);
            }
        }

    }

    public void ExitSight()
    {
        canSeePlayer = false;
        ResetAnimator();

        if (currState != BaddieState.Die)
            ReturnToIdle();
    }

    public void EnterRange()
    {
        ResetAnimator();

        if (currState == BaddieState.Idle || currState == BaddieState.Chasing)
            if (currState != BaddieState.Shooting)
            {
                StartCoroutine("AimBow", stats.aimTime);
            }
    }

    public void ExitRange()
    {
        if (currState != BaddieState.Shooting && currState != BaddieState.Die)
        {

            ResetAnimator();

            ReturnToIdle();

        }
    }

    public void ChasePlayer()
    {
        ResetAnimator();

        if (currState != BaddieState.Die)
        {
            currState = BaddieState.Chasing;

        }

    }

    public Projectile LoadBow()
    {
        Projectile arrowNumber = null;
        //spawn arrow

        foreach (Projectile p in Arrows)
        {
            if (p.alive)
            {
                arrowNumber = p;
                break;
            }
        }
        
        if (arrowNumber != null)
        {
            arrowNumber.gameObject.SetActive(true);
            return arrowNumber;
        }
        else
        {
            return null;
        }
    }
    public void DespawnArrow(Projectile _arrow )
    {
        //Despawn arrow
        _arrow.rb.useGravity = false;
        _arrow.rb.constraints = RigidbodyConstraints.FreezeAll;
        _arrow.transform.position = ArrowLoadSpot.transform.position;
        _arrow.transform.rotation = ArrowLoadSpot.transform.rotation;

        _arrow.transform.parent = ArrowLoadSpot.transform;
        _arrow.gameObject.SetActive(false);
    }

    public IEnumerator AimBow(float time)
    {
        ResetAnimator();
        currState = BaddieState.Aiming;
        Projectile _arrow = LoadBow();
        float timeLeft = time;

        //Debug.Log("arrow " + _arrow.gameObject.name);
        while (time> 0)
        {
            if (currState != BaddieState.Aiming)
            {
                DespawnArrow(_arrow);
                yield return null;
            }
            timeLeft -= Time.fixedDeltaTime;
           // Debug.Log("time till shot " + timeLeft);
            yield return new WaitForFixedUpdate();
        }


        FireBow(_arrow, stats.fireStrength);
        yield return null;
    }

    public void FireBow(Projectile _arrow, float punchForce)
    {
        
        ResetAnimator();
        currState = BaddieState.Shooting;
        _arrow.rb.useGravity = true;
        _arrow.rb.constraints = RigidbodyConstraints.None;
        _arrow.transform.position = ArrowLoadSpot.transform.position;
        _arrow.transform.rotation = ArrowLoadSpot.transform.rotation;

        _arrow.transform.parent = null;
        //punch
        _arrow.rb.AddForce(_arrow.transform.forward * punchForce, ForceMode.Impulse);
    }

    public void ReturnToIdle()
    {
        ResetAnimator(); 
        currState = BaddieState.Idle;
    }
    public void GetHurt(int ammount)
    {
        ResetAnimator();
        currState = BaddieState.Hurt;
        stats.health -= ammount;
        if (stats.health <= 0)
        {
            currState = BaddieState.Die;
        }
    }
    public void Die()
    {
      
        ResetAnimator();
        currState = BaddieState.Die;
    }


}
