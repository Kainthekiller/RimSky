using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIScriptVillage3 : MonoBehaviour
{
    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator anim;
    float speedMult;
    float detectRadius = 20;
    float fleeRadius = 10;
    GameObject player;
    AudioSource audiosource;
    public AudioClip[] clips;

    public bool GenderMale = false;

    void ResetAgent()
    {
        audiosource = this.gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        speedMult = Random.Range(0.1f, 1.5f);
        agent.speed = 2 * speedMult;
        agent.angularSpeed = 120;
        anim.SetFloat("speedMult", speedMult);
        anim.SetTrigger("isWalking");
        agent.ResetPath();
    }

    // Start is called before the first frame update
    void Start()
    {

        goalLocations = GameObject.FindGameObjectsWithTag("Village3Goals");
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        anim = this.GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0.0f, 1.0f));
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
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
        DetectMainChar();
    }
    public void DetectMainChar()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < detectRadius && (player.GetComponent<Animator>().GetBool("Attack1") == true || player.GetComponent<Animator>().GetBool("StrongAttack") || player.GetComponent<Animator>().GetBool("fireballAttack")))
        {
            AudioClip clip = GetRandomClip(); 
            if (GameManager.Instance.screamsfemalecurrent < GameManager.Instance.screamsfemalemax && !GenderMale)
            {
                audiosource.PlayOneShot(clip);
                GameManager.Instance.screamsfemalecurrent++;
            }
            if (GameManager.Instance.screamsmalecurrent < GameManager.Instance.screamsmalemax && GenderMale)
            {
                audiosource.PlayOneShot(clip);
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
