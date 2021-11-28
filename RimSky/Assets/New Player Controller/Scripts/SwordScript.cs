using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SwordScript : MonoBehaviour
{
    //Need the Box Collider to Turn on and off
    public int damageAmount = 50;
    public GameObject _swordCollider;
    public BoxCollider _Collider;
    public GameObject bloodEffect;
    bool strongAttack;
    private void Start()
    {
        _swordCollider = GameObject.Find("NewSwordCollider");
        _Collider = _swordCollider.GetComponent<BoxCollider>();
        _Collider.enabled = false;
    }

    IEnumerator  NPCDisapper(GameObject npc)
    {
        yield return new WaitForSeconds(3);
        Destroy(npc.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Destroy The Spawner
        if (collision.transform.tag == "NPCPeseant")
        {
            Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.identity);
            Animator NPCAnimation = collision.gameObject.GetComponent<Animator>();
            NavMeshAgent NPCnav = collision.gameObject.GetComponent<NavMeshAgent>();
            NPCAnimation.SetTrigger("isDead");
            NPCnav.isStopped = true;
            StartCoroutine(NPCDisapper(collision.gameObject));
        }



        if (collision.transform.tag == "Spawner")
        {
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "Archer" && strongAttack == true) 
        {

            Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.identity);
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamages(150);
        }

        else if (collision.transform.tag == "Archer")
        {
          
            Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.identity);
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamages(damageAmount);
        }

        if (collision.transform.tag == "KingsGuard" && strongAttack == true)
        {
            Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.identity);
            collision.gameObject.GetComponent<NewKGController>().TakeDamages(150);
        }

       else if (collision.transform.tag == "KingsGuard")
        {
            Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.identity);
         
            collision.gameObject.GetComponent<NewKGController>().TakeDamages(damageAmount);
        }

        if (collision.transform.tag == "Boss")
        {
            Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.identity);

            collision.gameObject.GetComponent<BossController>().TakeDamages(10);            
        }
        strongAttack = false;
    }

    public void StrongAttack()
    {
        strongAttack = true;
    }


    public void SwordBoxColliderEnabled()
    {
        _Collider.enabled = true;
    }

    public void SwordBoxColliderDisabled()
    {
        _Collider.enabled = false;
    }

}
