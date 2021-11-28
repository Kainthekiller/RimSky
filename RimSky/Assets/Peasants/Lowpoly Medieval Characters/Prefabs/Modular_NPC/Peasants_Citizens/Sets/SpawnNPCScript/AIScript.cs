using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIScript : MonoBehaviour
{
    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator anim;
    float speedMult;
    float detectRadius = 20;
    float fleeRadius = 10;
    GameObject player;
    GameObject pauseMenu;
   public AudioClip[] clips;
   public bool GenderMale = false;
    AudioSource audioSource;
    void ResetAgent()
    {
       
        player = GameObject.Find("Player");
        speedMult = Random.Range(0.1f, 1.5f);
        agent.speed = 2 * speedMult;
        agent.angularSpeed = 120;
        anim.SetFloat("speedMult", speedMult);
        anim.SetTrigger("isWalking");
        agent.ResetPath();
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("Pause");
        audioSource = this.gameObject.GetComponent<AudioSource>();
        goalLocations = GameObject.FindGameObjectsWithTag("SpawnGoals");
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        anim = this.GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0.0f, 1.0f));
        anim.SetTrigger("isWalking");
        ResetAgent();
        if (agent.speed < 1.5)
        {
            agent.speed = 1.5f;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 1) 
        {
            if (goalLocations != null)
            {
                agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
            }
           
        }

        if (pauseMenu.activeInHierarchy == false)
        {
            DetectMainChar();
        }      
    }
    public void DetectMainChar()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < detectRadius && (player.GetComponent<Animator>().GetBool("JumpAttack") == true || player.GetComponent<Animator>().GetBool("StrongAttack") || player.GetComponent<Animator>().GetBool("fireballAttack") || player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack1")))
        {
            AudioClip clip = GetRandomClip();
            if (GameManager.Instance.screamsfemalecurrent < GameManager.Instance.screamsfemalemax && !GenderMale )
            {
                audioSource.PlayOneShot(clip);
                GameManager.Instance.screamsfemalecurrent++;
            }
            if (GameManager.Instance.screamsmalecurrent < GameManager.Instance.screamsmalemax && GenderMale)
            {
                audioSource.PlayOneShot(clip);
                GameManager.Instance.screamsmalecurrent++;
            }
            Vector3 fleeDirection = (this.transform.position - player.transform.position).normalized;
            Vector3 newGoal = this.transform.position + fleeDirection * fleeRadius;
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newGoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                anim.SetTrigger("isRunning");
                agent.speed = 5;
                agent.angularSpeed = 500;
            }
            
        }
    }
    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }


}
